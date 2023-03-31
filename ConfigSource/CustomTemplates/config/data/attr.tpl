{{~if table.is_map_table ~}}
namespace {{table.namespace_with_top_module}}
{
    public static partial class NumericType
    {
{{~for d in datas~}}
        {{fieldName = get_field d "Field" | string.replace '"' ""}}
        /// <summary>
        /// {{get_field d "Name"| string.replace '"' ""}}
        /// </summary>
        public const int {{fieldName}} = {{get_field d table.index}};
        {{isNumeric = get_field d 'IsNumeric'}}
    {{~if isNumeric.value ~}}
        public const int {{fieldName}}Base      = {{fieldName}} * 10 + 1;
        public const int {{fieldName}}Add       = {{fieldName}} * 10 + 2;
        public const int {{fieldName}}Pct       = {{fieldName}} * 10 + 3;
        public const int {{fieldName}}FinalAdd  = {{fieldName}} * 10 + 4;
        public const int {{fieldName}}FinalPct  = {{fieldName}} * 10 + 5;
    {{~end~}}
{{~end~}}
    }
}
{{~end~}}