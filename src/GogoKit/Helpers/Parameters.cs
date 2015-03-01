using System.Collections.Generic;
using GogoKit.Requests;

namespace GogoKit.Helpers
{
    public class Parameters
    {
        private readonly IDictionary<string, string> _parameters;

        public static Parameters WithPaging(PageRequest pageRequest)
        {
            Requires.ArgumentNotNull(pageRequest, "pageRequest");

            var parameters = new Dictionary<string, string>
                             {
                                {"page", pageRequest.Page.GetValueOrDefault(1).ToString() },
                                {"page_size", pageRequest.PageSize.GetValueOrDefault(30).ToString() }
                             };
            return new Parameters(parameters);
        }

        private Parameters(IDictionary<string, string> parameters)
        {
            Requires.ArgumentNotNull(parameters, "parameters");

            _parameters = parameters;
        }

        public Parameters And(string name, string value)
        {
            Requires.ArgumentNotNull(name, "name");

            _parameters.Add(name, value ?? "");
            return this;
        }

        public IDictionary<string, string> Build()
        {
            return _parameters;
        }
    }
}
