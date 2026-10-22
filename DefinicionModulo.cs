namespace SistemaControlClinicoVeterinarioV2.Formularios;

public sealed record Campo(string Columna, string Etiqueta, bool Obligatorio = false, bool Multilinea = false, string Tipo = "texto");
public sealed record DefinicionModulo(string Titulo, string Tabla, string Consulta, Campo[] Campos)
{
    public static readonly DefinicionModulo[] Todos =
    {
        new("Propietarios", "Propietarios", "SELECT Id,Nombre,DPI,Telefono,Correo FROM Propietarios ORDER BY Nombre", new[] {
            new Campo("Nombre","Nombre completo",true), new Campo("DPI","DPI",false), new Campo("Telefono","Teléfono",true), new Campo("Correo","Correo electrónico") }),
        new("Mascotas", "Mascotas", "SELECT m.Id,m.Nombre,m.Especie,m.Raza,m.Sexo,m.FechaNacimiento,m.Peso,m.PropietarioId,p.Nombre AS Propietario,m.Observaciones FROM Mascotas m JOIN Propietarios p ON p.Id=m.PropietarioId ORDER BY m.Nombre", new[] {
            new Campo("Nombre","Nombre de mascota",true), new Campo("Especie","Especie (perro, gato...)",true), new Campo("Raza","Raza"), new Campo("Sexo","Sexo"), new Campo("FechaNacimiento","Nacimiento (AAAA-MM-DD)"), new Campo("Peso","Peso en kg",false,false,"numero"), new Campo("PropietarioId","ID del propietario",true,false,"entero"), new Campo("Observaciones","Observaciones",false,true) }),
        new("Veterinarios", "Veterinarios", "SELECT Id,Nombre,Colegiado,Telefono,Correo,Especialidad FROM Veterinarios ORDER BY Nombre", new[] {
            new Campo("Nombre","Nombre completo",true), new Campo("Colegiado","No. colegiado"), new Campo("Telefono","Teléfono"), new Campo("Correo","Correo electrónico"), new Campo("Especialidad","Especialidad") }),
        new("Citas", "Citas", "SELECT c.Id,c.MascotaId,m.Nombre AS Mascota,c.VeterinarioId,v.Nombre AS Veterinario,c.FechaHora,c.Motivo,c.Estado FROM Citas c JOIN Mascotas m ON m.Id=c.MascotaId JOIN Veterinarios v ON v.Id=c.VeterinarioId ORDER BY c.FechaHora DESC", new[] {
            new Campo("MascotaId","ID de mascota",true,false,"entero"), new Campo("VeterinarioId","ID de veterinario",true,false,"entero"), new Campo("FechaHora","Fecha y hora (AAAA-MM-DD HH:MM)",true), new Campo("Motivo","Motivo de consulta",true,true), new Campo("Estado","Estado: Programada, Atendida o Cancelada",true) }),
        new("Historial clínico", "ConsultasClinicas", "SELECT cc.Id,cc.CitaId,cc.MascotaId,m.Nombre AS Mascota,cc.Fecha,cc.Sintomas,cc.Diagnostico,cc.Tratamiento,cc.Medicamentos,cc.ProximaVisita FROM ConsultasClinicas cc JOIN Mascotas m ON m.Id=cc.MascotaId ORDER BY cc.Fecha DESC", new[] {
            new Campo("CitaId","ID de cita (opcional)",false,false,"enteroOpcional"), new Campo("MascotaId","ID de mascota",true,false,"entero"), new Campo("Fecha","Fecha (AAAA-MM-DD)",true), new Campo("Sintomas","Síntomas",true,true), new Campo("Diagnostico","Diagnóstico",true,true), new Campo("Tratamiento","Tratamiento indicado",false,true), new Campo("Medicamentos","Medicamentos / dosis",false,true), new Campo("ProximaVisita","Próxima visita (AAAA-MM-DD)") })
    };
}
