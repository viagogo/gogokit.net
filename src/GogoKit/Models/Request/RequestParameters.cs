using System;
using System.Collections.Generic;
using System.Linq;
using HalKit.Models.Request;

namespace GogoKit.Models.Request
{
    /// <summary>
    /// Base class for classes that represent query parameters and headers to a
    /// particular API endpoint
    /// </summary>
    public abstract class RequestParameters<TEmbed, TSort> : IRequestParameters
    {
        private readonly IDictionary<string, string> _parameters;
        private readonly IDictionary<string, IEnumerable<string>> _headers;

        protected RequestParameters()
        {
            _parameters = new Dictionary<string, string>();
            _headers = new Dictionary<string, IEnumerable<string>>();
        }

        public IDictionary<string, string> Parameters
        {
            get { return _parameters; }
        }

        public IDictionary<string, IEnumerable<string>> Headers
        {
            get { return _headers; }
        }

        public Guid? IdempotencyKey
        {
            get
            {
                Guid idempotencyKey;
                IEnumerable<string> textValues;
                if (!_headers.TryGetValue("Idempotency-Key", out textValues) ||
                    !Guid.TryParse(textValues.First(), out idempotencyKey))
                {
                    return null;
                }

                return idempotencyKey;
            }
            set
            {
                _headers.Add("Idempotency-Key",
                             value.HasValue
                                 ? new[] {value.ToString()}
                                 : null);
            }
        }

        public int? PageSize
        {
            get { return GetParameter("page_size", int.Parse); }
            set { SetParameter("page_size", value); }
        }

        public int? Page
        {
            get { return GetParameter("page", int.Parse); }
            set { SetParameter("page", value); }
        }

        public IEnumerable<TEmbed> Embed
        {
            get
            {
                string embed;
                if (!_parameters.TryGetValue("embed", out embed))
                {
                    yield break;
                }

                var fieldNameMap = EmbedFieldNameMap.ToDictionary(kv => kv.Value, kv => kv.Key);
                var fieldNames = embed.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries);
                foreach (var fieldName in fieldNames)
                {
                    yield return fieldNameMap[fieldName];
                }
            }
            set
            {
                var embedValues = value != null 
                                    ? value.Select(e => EmbedFieldNameMap[e])
                                    : null;
                SetParameter("embed", string.Join(",", embedValues ?? new string[] {}));
            }
        }

        public IEnumerable<Sort<TSort>> Sort
        {
            get
            {
                string valueText;
                if (!_parameters.TryGetValue("sort", out valueText) || valueText == null)
                {
                    yield break;
                }

                var fieldNameSortMap = SortFieldNameMap.ToDictionary(kv => kv.Value, kv => kv.Key);
                var sortStrings = valueText.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var sort in sortStrings)
                {
                    var direction = SortDirection.Ascending;
                    var fieldName = sort;
                    if (sort.StartsWith("-"))
                    {
                        direction = SortDirection.Descending;
                        fieldName = sort.Remove(0, 1);
                    }

                    yield return new Sort<TSort>(fieldNameSortMap[fieldName], direction);
                }
            }
            set
            {
                var sortStrings = new List<string>();
                foreach (var sort in value ?? new Sort<TSort>[] {})
                {
                    var sortString = string.Format("{0}{1}",
                                                   sort.Direction == SortDirection.Descending ? "-" : "",
                                                   SortFieldNameMap[sort.Field]);
                    sortStrings.Add(sortString);
                }

                SetParameter("sort", string.Join(",", sortStrings));
            }
        }

        protected virtual IDictionary<TEmbed, string> EmbedFieldNameMap
        {
            get { return new Dictionary<TEmbed, string>(); }
        }

        protected virtual IDictionary<TSort, string> SortFieldNameMap
        {
            get { return new Dictionary<TSort, string>(); }
        }

        protected T? GetParameter<T>(string key, Func<string, T> parseFunc)
            where T : struct
        {
            string valueText;
            if (!_parameters.TryGetValue(key, out valueText))
            {
                return null;
            }

            return parseFunc(valueText);
        }

        protected void SetParameter<T>(string key, T? value)
            where T : struct
        {
            SetParameter(key, value.HasValue ? value.ToString() : null);
        }

        protected void SetParameter(string key, string value)
        {
            if (_parameters.ContainsKey(key))
            {
                _parameters[key] = value;
            }

            _parameters.Add(key, value);
        }
    }

    public class Sort<T>
    {
        private readonly T _field;
        private readonly SortDirection _direction;

        public Sort(T field, SortDirection direction)
        {
            Requires.ArgumentNotNull(field, nameof(field));
            Requires.ArgumentNotNull(direction, nameof(direction));

            _field = field;
            _direction = direction;
        }

        public T Field
        {
            get { return _field; }
        }

        public SortDirection Direction
        {
            get { return _direction; }
        }
    }

    public enum SortDirection
    {
        Ascending,
        Descending
    }
}
