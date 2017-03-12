using System;

namespace MovieMeApp.Utils
{
    public class StringUtils
    {
        /// <summary>
        /// Gets the name of the category.
        /// </summary>
        /// <returns>The category name.</returns>
        /// <param name="id">Identifier.</param>
        /// 
        public string GetCategoryName (string id)
        {
            //TODO: EY! this is a horrible hack, because of time I have trouble tring to access to resources by name to get its values, 
            //so I priorize to develop more relevant things than spent time on this.
            switch (id) 
            {
                case "top_rated": 
                        return "Top Rated";
                case "popular": 
                        return "Popular";
                case "similar": 
                        return "Similar Movies";
				case "now_playing":
						return "Now Playing";
                default:
                    return string.Empty;
            }
        }
    }
}
