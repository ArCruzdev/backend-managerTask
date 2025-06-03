namespace Application.Common.Mappings;

public interface IMapFrom<T>
{
    // Por convención, AutoMapper buscará un método estático llamado 'Mapping'
    // en la clase que implementa esta interfaz.
    // public void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());
    // Pero para un mapeo simple de A a B, la interfaz vacía es suficiente si los nombres de propiedades coinciden.
    // AutoMapper es inteligente y mapeará automáticamente por convención de nombres.
    // Si necesitas reglas de mapeo más complejas, las definirás en un Profile separado.
}
