using HalKit.Json;
using HalKit.Models.Response;

namespace GogoKit.Models.Response
{
    /// <summary>
    /// The root of the viagogo API service. Applications can use the links provided
    /// by this resource to find all other resources in the API.
    /// </summary>
    public class Root : Resource
    {
        /// <summary>
        /// You can GET the href of this link to retrieve the available
        /// <see cref="Country"/> resources.
        /// </summary>
        /// <remarks>See http://developer.viagogo.net/#viagogocountries.</remarks>
        [Rel("viagogo:countries")]
        public Link CountriesLink { get; set; }

        /// <summary>
        /// You can GET the href of this link to retrieve the Currencies available
        /// <see cref="Currency"/> resources.
        /// </summary>
        /// <remarks>See http://developer.viagogo.net/#viagogocurrencies. </remarks>
        [Rel("viagogo:currencies")]
        public Link CurrenciesLink { get; set; }

        /// <summary>
        /// You can GET the href of this link to retrieve the genre <see cref="Category"/>
        /// resources (e.g. “Sports”, “Concerts”).
        /// </summary>
        /// <remarks>See http://developer.viagogo.net/#viagogogenres.</remarks>
        [Rel("viagogo:genres")]
        public Link GenresLink { get; set; }

        /// <summary>
        /// You can GET the href of this link to retrieve the available <see cref="Language"/>.
        /// resources
        /// </summary>
        /// <remarks>See http://developer.viagogo.net/#viagogolanguages. </remarks>
        [Rel("viagogo:languages")]
        public Link LanguagesLink { get; set; }

        /// <summary>
        /// You can GET the href of this link to retrieve the available <see cref="MetroArea"/>
        /// resources.
        /// </summary>
        /// <remarks>See http://developer.viagogo.net/#viagogometroareas. </remarks>
        [Rel("viagogo:metroareas")]
        public Link MetroAreasLink { get; set; }

        /// <summary>
        /// You can GET the href of this link to search for entities that
        /// match a given query.
        /// </summary>
        /// <remarks>See http://developer.viagogo.net/#viagogosearch. </remarks>
        [Rel("viagogo:search")]
        public Link SearchLink { get; set; }

        /// <summary>
        /// You can GET the href of this link to retrieve the available <see cref="Venue"/>
        /// resources.
        /// </summary>
        /// <remarks>See http://developer.viagogo.net/#viagogovenues. </remarks>
        [Rel("viagogo:venues")]
        public Link VenuesLink { get; set; }
        
        /// <summary>
        /// You can GET the href of this link to retrieve the current <see cref="User"/>
        /// resource.
        /// </summary>
        /// <remarks>See http://developer.viagogo.net/#viagogouser. </remarks>
        [Rel("viagogo:user")]
        public Link UserLink { get; set; }
    }
}
