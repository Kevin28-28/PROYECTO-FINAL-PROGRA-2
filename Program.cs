using SistemaControlClinicoVeterinarioV2.Formularios;
using SistemaControlClinicoVeterinarioV2.Persistencia;

namespace SistemaControlClinicoVeterinarioV2;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        try
        {
            var db = new InicializadorBaseDatos();
            db.Inicializar();
            Application.Run(new FrmPrincipal(new RepositorioClinico(db.CadenaConexion)));
        }
        catch (Exception ex)
        {
            MessageBox.Show($"No fue posible iniciar el sistema:
{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
