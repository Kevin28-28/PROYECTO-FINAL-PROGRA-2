using SistemaControlClinicoVeterinarioV2.Persistencia;

namespace SistemaControlClinicoVeterinarioV2.Formularios;

public sealed class FrmPrincipal : Form
{
    private readonly RepositorioClinico _repo;
    public FrmPrincipal(RepositorioClinico repo)
    {
        _repo=repo; Text="Sistema de Control Clínico Veterinario v2"; Width=1050; Height=680; MinimumSize=new Size(850,550); StartPosition=FormStartPosition.CenterScreen;
        var header=new Panel { Dock=DockStyle.Top, Height=100, BackColor=Color.FromArgb(23,75,82) };
        header.Controls.Add(new Label { Text="CONTROL CLÍNICO VETERINARIO", ForeColor=Color.White, Font=new Font("Segoe UI",20,FontStyle.Bold), AutoSize=true, Location=new Point(24,18) });
        header.Controls.Add(new Label { Text="Propietarios · Mascotas · Veterinarios · Citas · Historial clínico", ForeColor=Color.WhiteSmoke, AutoSize=true, Location=new Point(28,62) });
        var tabs=new TabControl { Dock=DockStyle.Fill, Padding=new Point(14,7) };
        foreach(var def in DefinicionModulo.Todos) { var tab=new TabPage(def.Titulo); tab.Controls.Add(new FrmCrudModulo(_repo,def){Dock=DockStyle.Fill,TopLevel=false,FormBorderStyle=FormBorderStyle.None}); var child=(FrmCrudModulo)tab.Controls[0]; child.Show(); tabs.TabPages.Add(tab); }
        var footer=new Label { Dock=DockStyle.Bottom, Height=28, Text="Base de datos SQLite local · Use los ID relacionados visibles en las tablas para enlazar registros.", TextAlign=ContentAlignment.MiddleLeft, Padding=new Padding(10,0,0,0), BackColor=Color.Gainsboro };
        Controls.Add(tabs); Controls.Add(footer); Controls.Add(header);
    }
}
