using System.Globalization;
using System.Data;
using SistemaControlClinicoVeterinarioV2.Persistencia;

namespace SistemaControlClinicoVeterinarioV2.Formularios;

public sealed class FrmCrudModulo : Form
{
    private readonly RepositorioClinico _repo; private readonly DefinicionModulo _def;
    private readonly DataGridView _grid=new() { Dock=DockStyle.Fill, ReadOnly=true, MultiSelect=false, SelectionMode=DataGridViewSelectionMode.FullRowSelect, AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.DisplayedCells, AllowUserToAddRows=false, BackgroundColor=Color.White };
    private readonly Dictionary<string,Control> _inputs=new(); private readonly Panel _editor=new() { Dock=DockStyle.Right, Width=320, AutoScroll=true, Padding=new Padding(12) };
    private int? _selectedId;
    public FrmCrudModulo(RepositorioClinico repo, DefinicionModulo def) { _repo=repo; _def=def; Build(); LoadData(); }
    private void Build()
    {
        var split=new SplitContainer { Size=new Size(1000,500), Dock=DockStyle.Fill, FixedPanel=FixedPanel.Panel2, Panel2MinSize=280, SplitterDistance=650 };
        split.Panel1.Controls.Add(_grid); split.Panel2.Controls.Add(_editor); Controls.Add(split);
        var y=8; _editor.Controls.Add(new Label{Text="Datos del registro",Font=new Font("Segoe UI",12,FontStyle.Bold),AutoSize=true,Location=new Point(12,y)}); y+=34;
        foreach(var campo in _def.Campos)
        {
            _editor.Controls.Add(new Label{Text=campo.Etiqueta+(campo.Obligatorio?" *":""),AutoSize=true,Location=new Point(12,y)}); y+=20;
            Control input=campo.Multilinea ? new TextBox{Multiline=true,Height=62,ScrollBars=ScrollBars.Vertical} : new TextBox{Height=25};
            input.Location=new Point(12,y); input.Width=260; _inputs[campo.Columna]=input; _editor.Controls.Add(input); y+=input.Height+9;
        }
        var btnPanel=new FlowLayoutPanel{Location=new Point(8,y),Width=285,Height=100};
        btnPanel.Controls.Add(Btn("Nuevo / Limpiar",(_,_)=>Limpiar())); btnPanel.Controls.Add(Btn("Guardar",(_,_)=>Guardar())); btnPanel.Controls.Add(Btn("Eliminar",(_,_)=>Eliminar())); btnPanel.Controls.Add(Btn("Actualizar lista",(_,_)=>LoadData())); _editor.Controls.Add(btnPanel);
        _grid.SelectionChanged+=(_,_)=>SeleccionarFila();
    }
    private static Button Btn(string text,EventHandler click){var b=new Button{Text=text,AutoSize=true,Height=32,Margin=new Padding(3)};b.Click+=click;return b;}
    private void LoadData()
    {
        try { _grid.DataSource=_repo.Consultar(_def.Consulta); if(_grid.Columns.Contains("Id"))_grid.Columns["Id"].Width=55; _grid.ClearSelection(); }
        catch(Exception ex){Error(ex);}
    }
    private void SeleccionarFila()
    {
        if(_grid.CurrentRow?.DataBoundItem is not DataRowView row)return;
        try { _selectedId=Convert.ToInt32(row["Id"]); foreach(var campo in _def.Campos) { var v=row.Row.Table.Columns.Contains(campo.Columna)?row[campo.Columna]:DBNull.Value; _inputs[campo.Columna].Text=v is DBNull?"":Convert.ToString(v,CultureInfo.InvariantCulture)??""; } }
        catch { }
    }
    private void Limpiar(){_selectedId=null;foreach(var c in _inputs.Values)c.Text="";_grid.ClearSelection();}
    private void Guardar()
    {
        try
        {
            var values=new List<object?>();
            foreach(var f in _def.Campos)
            {
                var s=_inputs[f.Columna].Text.Trim(); if(f.Obligatorio&&s.Length==0)throw new ArgumentException($"El campo '{f.Etiqueta}' es obligatorio.");
                object? value=s.Length==0?null:s;
                if(s.Length>0&&f.Tipo=="entero") { if(!int.TryParse(s,out var n)||n<=0)throw new ArgumentException($"'{f.Etiqueta}' debe ser un ID entero positivo."); value=n; }
                if(s.Length>0&&f.Tipo=="enteroOpcional") { if(!int.TryParse(s,out var n)||n<=0)throw new ArgumentException($"'{f.Etiqueta}' debe ser un ID entero positivo o quedar vacío."); value=n; }
                if(s.Length>0&&f.Tipo=="numero") { if(!double.TryParse(s,NumberStyles.Number,CultureInfo.InvariantCulture,out var n)||n<=0)throw new ArgumentException($"'{f.Etiqueta}' debe ser un número mayor que cero. Use punto decimal."); value=n; }
                values.Add(value);
            }
            if(_selectedId.HasValue) { if(_repo.Actualizar(_def.Tabla,_selectedId.Value,_def.Campos.Select(c=>c.Columna).ToArray(),values.ToArray())==0)throw new InvalidOperationException("No existe el registro seleccionado."); MessageBox.Show("Registro actualizado correctamente."); }
            else { var id=_repo.Crear(_def.Tabla,_def.Campos.Select(c=>c.Columna).ToArray(),values.ToArray()); MessageBox.Show($"Registro creado. ID: {id}"); }
            Limpiar(); LoadData();
        }
        catch(Exception ex){Error(ex);}
    }
    private void Eliminar()
    {
        if(!_selectedId.HasValue){MessageBox.Show("Seleccione un registro de la tabla.");return;}
        if(MessageBox.Show("¿Desea eliminar el registro seleccionado?","Confirmar",MessageBoxButtons.YesNo,MessageBoxIcon.Warning)!=DialogResult.Yes)return;
        try { if(_repo.Eliminar(_def.Tabla,_selectedId.Value)==0)throw new InvalidOperationException("No existe el registro seleccionado."); Limpiar();LoadData();MessageBox.Show("Registro eliminado."); }
        catch(Exception ex){Error(ex);}
    }
    private static void Error(Exception ex)=>MessageBox.Show(ex.Message,"No se pudo completar la operación",MessageBoxButtons.OK,MessageBoxIcon.Error);
}
