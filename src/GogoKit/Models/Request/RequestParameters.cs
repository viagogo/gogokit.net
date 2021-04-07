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
        protected RequestParameters()
        {
            Parameters = new Dictionary<string, string>();
            Headers = new Dictionary<string, IEnumerable<string>>();
        }

        public IDictionary<string, string> Parameters { get; }

        public IDictionary<string, IEnumerable<string>> Headers { get; }

        public Guid? IdempotencyKey
        {
            get
            {
                Guid idempotencyKey;
                IEnumerable<string> textValues;
                if (!Headers.TryGetValue("Idempotency-Key", out textValues) ||
                    !Guid.TryParse(textValues.First(), out idempotencyKey))
                {
                    return null;
                }

                return idempotencyKey;
            }
            set
            {
                Headers.Add("Idempotency-Key",
                            value.HasValue ? new[] {value.ToString()} : null);
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
                if (!Parameters.TryGetValue("embed", out embed))
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
                var embedValues = value?.Select(e => EmbedFieldNameMap[e]);
                SetParameter("embed", string.Join(",", embedValues ?? new string[] {}));
            }
        }

        public IEnumerable<Sort<TSort>> Sort
        {
            get
            {
                string valueText;
                if (!Parameters.TryGetValue("sort", out valueText) || valueText == null)
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
                    var sortString = (sort.Direction == SortDirection.Descending ? "-" : "") +
                                     SortFieldNameMap[sort.Field];
                    sortStrings.Add(sortString);
                }

                SetParameter("sort", string.Join(",", sortStrings));
            }
        }

        protected virtual IDictionary<TEmbed, string> EmbedFieldNameMap => new Dictionary<TEmbed, string>();

        protected virtual IDictionary<TSort, string> SortFieldNameMap => new Dictionary<TSort, string>();

        protected T? GetParameter<T>(string key, Func<string, T> parseFunc)
            where T : struct
        {
            string valueText;
            if (!Parameters.TryGetValue(key, out valueText))
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
            if (Parameters.ContainsKey(key))
            {
                Parameters[key] = value;
            }

            Parameters.Add(key, value);
        }
    }

    public class Sort<T>
    {
        public Sort(T field, SortDirection direction)
        {
            Requires.ArgumentNotNull(field, nameof(field));
            Requires.ArgumentNotNull(direction, nameof(direction));

            Field = field;
            Direction = direction;
        }

        public T Field { get; }

        public SortDirection Direction { get; }
    }

    public enum SortDirection
    {
        Ascending,
        Descending
    }
}
