namespace ApiBanco.Repositorio
{
    public class Notificador
    {
        public void EnviarNotificacion(string mensaje)
        {
            // Aquí puedes integrar un sistema de correo, SMS, o simplemente loguear
            Console.WriteLine($"Notificación: {mensaje}");
        }
    }
}
