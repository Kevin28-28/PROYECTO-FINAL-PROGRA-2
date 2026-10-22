using System.Reflection;
using Microsoft.Data.Sqlite;

namespace SistemaControlClinicoVeterinarioV2.Persistencia;

public sealed class InicializadorBaseDatos
{
    public string RutaBaseDatos { get; } = Environment.GetEnvironmentVariable("VETERINARIA_DB") ??
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "UMG", "VeterinariaV2", "veterinaria.db");
    public string CadenaConexion => new SqliteConnectionStringBuilder { DataSource = RutaBaseDatos, ForeignKeys = true }.ToString();

    public void Inicializar()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(RutaBaseDatos)!);
        using var cn = new SqliteConnection(CadenaConexion); cn.Open();
        using var cmd = cn.CreateCommand();
        var asm = Assembly.GetExecutingAssembly();
        var recurso = asm.GetManifestResourceNames().Single(n => n.EndsWith("crear_tablas.sql", StringComparison.OrdinalIgnoreCase));
        using var stream = asm.GetManifestResourceStream(recurso) ?? throw new InvalidOperationException("No se encontró crear_tablas.sql.");
        using var reader = new StreamReader(stream); cmd.CommandText = reader.ReadToEnd(); cmd.ExecuteNonQuery();
    }
}
