using System.Data;
using Microsoft.Data.Sqlite;

namespace SistemaControlClinicoVeterinarioV2.Persistencia;

// Acceso a datos SQLite: todos los valores se envían como parámetros.
public sealed class RepositorioClinico
{
    private readonly string _cs;
    public RepositorioClinico(string cadenaConexion) => _cs = cadenaConexion;

    public DataTable Consultar(string sql, params (string Nombre, object? Valor)[] parametros)
    {
        using var cn = Abrir(); using var cmd = cn.CreateCommand(); cmd.CommandText = sql; Parametros(cmd, parametros);
        using var rd = cmd.ExecuteReader(); var tabla = new DataTable(); tabla.Load(rd); return tabla;
    }
    public int Ejecutar(string sql, params (string Nombre, object? Valor)[] parametros)
    {
        using var cn = Abrir(); using var cmd = cn.CreateCommand(); cmd.CommandText = sql; Parametros(cmd, parametros); return cmd.ExecuteNonQuery();
    }
    public long Crear(string tabla, string[] columnas, object?[] valores)
    {
        ValidarTabla(tabla); if (columnas.Length == 0 || columnas.Length != valores.Length) throw new ArgumentException("Datos de formulario no válidos.");
        foreach (var c in columnas) if (!System.Text.RegularExpressions.Regex.IsMatch(c, @"^[A-Za-z][A-Za-z0-9]*$")) throw new ArgumentException("Columna no válida.");
        var sql = $"INSERT INTO {tabla} ({string.Join(",", columnas)}) VALUES ({string.Join(",", columnas.Select((_,i)=>"$p"+i))}); SELECT last_insert_rowid();";
        using var cn = Abrir(); using var cmd = cn.CreateCommand(); cmd.CommandText = sql;
        for (int i=0;i<valores.Length;i++) cmd.Parameters.AddWithValue("$p"+i, valores[i] ?? DBNull.Value);
        return (long)cmd.ExecuteScalar()!;
    }
    public int Actualizar(string tabla, int id, string[] columnas, object?[] valores)
    {
        ValidarTabla(tabla); if (columnas.Length != valores.Length) throw new ArgumentException("Datos no válidos.");
        foreach (var c in columnas) if (!System.Text.RegularExpressions.Regex.IsMatch(c, @"^[A-Za-z][A-Za-z0-9]*$")) throw new ArgumentException("Columna no válida.");
        var sql=$"UPDATE {tabla} SET {string.Join(",",columnas.Select((c,i)=>$"{c}=$p{i}"))} WHERE Id=$id";
        using var cn=Abrir(); using var cmd=cn.CreateCommand(); cmd.CommandText=sql;
        for(int i=0;i<valores.Length;i++) cmd.Parameters.AddWithValue("$p"+i,valores[i]??DBNull.Value);
        cmd.Parameters.AddWithValue("$id",id); return cmd.ExecuteNonQuery();
    }
    public int Eliminar(string tabla, int id) { ValidarTabla(tabla); return Ejecutar($"DELETE FROM {tabla} WHERE Id=$id", ("$id",id)); }
    private SqliteConnection Abrir() { var cn=new SqliteConnection(_cs); cn.Open(); return cn; }
    private static void Parametros(SqliteCommand cmd, (string Nombre, object? Valor)[] ps) { foreach(var p in ps) cmd.Parameters.AddWithValue(p.Nombre,p.Valor??DBNull.Value); }
    private static void ValidarTabla(string t) { if (t is not ("Propietarios" or "Mascotas" or "Veterinarios" or "Citas" or "ConsultasClinicas")) throw new ArgumentException("Tabla no permitida."); }
}
