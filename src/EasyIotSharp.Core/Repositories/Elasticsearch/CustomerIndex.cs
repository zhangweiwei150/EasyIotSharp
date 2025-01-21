using Elasticsearch.Net;
using Nest;
using System.Collections.Generic;
using EasyIotSharp.Core;

namespace EasyIotSharp.Repositories.Elasticsearch
{
    /// <summary>
    /// es 自定义索引
    /// </summary>
    public class CustomerIndex
    {
        /// <summary>
        ///初始化Settings
        /// </summary>
        public static IndexSettings Settings = new IndexSettings() { NumberOfReplicas = 3, NumberOfShards = 1 };

        /// <summary>
        /// 自定义setting
        /// </summary>
        public static IndexSettings DefaultSettings
        {
            get
            {
                var settings = Settings;

                var analysis = new
                {
                    analyzer = new
                    {
                        pinyin_analyzer = new
                        {
                            tokenizer = "my_pinyin"
                        }
                    },
                    tokenizer = new
                    {
                        my_pinyin = new
                        {
                            type = "pinyin",
                            keep_separate_first_letter = false,
                            keep_full_pinyin = true,
                            keep_original = false,
                            limit_first_letter_length = 16,
                            lowercase = true,
                            remove_duplicated_term = true
                        }
                    }
                };

                settings.Add("analysis", analysis);
                settings.Add("max_result_window", GlobalConsts.ESSettings.MAXRESULTWINDOW);
                return settings;
            }
            set { }
        }

        /// <summary>
        /// custom mapping property
        /// </summary>
        public class MappingProperty : IProperty
        {
            /// <summary>
            ///
            /// </summary>
            public MappingProperty()
            {
            }

            /// <summary>
            ///
            /// </summary>
            /// <param name="filed"></param>
            /// <param name="type"></param>
            public MappingProperty(string filed, Nest.FieldType type = FieldType.SearchAsYouType)
            {
                Name = filed;
                Type = type.GetStringValue();

                if (type != FieldType.SearchAsYouType) return;

                Type = FieldType.Text.GetStringValue();
                Fields = new
                {
                    Keyword = new { Type = FieldType.Keyword.GetStringValue(), Ignore_above = 256 },
                    Alias = new
                    {
                        Type = FieldType.Text.GetStringValue(),
                        Analyzer = "ik_max_word",
                        Search_analyzer = "ik_smart"
                    },
                    Pinyin = new
                    {
                        Type = FieldType.Text.GetStringValue(),
                        store = false,
                        Term_vector = "with_offsets",
                        Analyzer = "pinyin_analyzer",
                        Boost = 10
                    }
                };
            }

            /// <summary>
            ///
            /// </summary>
            public IDictionary<string, object> LocalMetadata { get; set; }

            /// <summary>
            ///
            /// </summary>
            public IDictionary<string, string> Meta { get; set; }

            /// <summary>
            ///
            /// </summary>
            public PropertyName Name { get; set; }

            /// <summary>
            ///
            /// </summary>
            public string Type { get; set; }

            /// <summary>
            ///
            /// </summary>
            public object Fields { get; set; }
        }
    }
}