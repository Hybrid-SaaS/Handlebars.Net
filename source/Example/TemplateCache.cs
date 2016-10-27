using System;
using System.Collections.Generic;
using System.IO;
using HandlebarsDotNet;

namespace Example
{
    public static class TemplateCache
    {
        public static Dictionary<string, Action<TextWriter, object>> PartialCache { private get; set; }

        public static IHandlebars GetHandleBars(State state)
        {
            var customConfig = new CustomConfig
            {
                State = state
            };

            var handlebars = Handlebars.Create( customConfig );
                handlebars.RegisterHelper( "outputLanguage", OutputLanguage );

            LoadTemplates( handlebars, customConfig );
          
            return handlebars;
        }

        private static void LoadTemplates( IHandlebars handlebars, CustomConfig customConfig )
        {
            if ( PartialCache == null )
            {
                PartialCache = new Dictionary<string, Action<TextWriter, object>>();
                for (var x = 0; x < 100; x++)
                {
                    var template = CreateTemplate(handlebars,  $@"
<div style='margin: 1px; border: solid 1px silver; padding: 2px; font: 10pt Tahoma'>
    partial #{x}<br>
    Language (Data): {{{{language}}}}<br>
    Language (Closure): {customConfig.State.Language}<br>
    Language (Helper): {{{{outputLanguage}}}}
</div>
");
                    PartialCache.Add($"partial{x}", template);
                }
            }

            foreach ( var partial in PartialCache )
            {
                handlebars.RegisterTemplate( partial.Key, partial.Value );
            }
        }


        private static Action<TextWriter, object> CreateTemplate( IHandlebars handlebars, string template )
        {
            using ( var reader = new StringReader( template ) )
            {
                return handlebars.Compile( reader );
            }
        }

        private static void OutputLanguage( TextWriter output, dynamic context, HandlebarsConfiguration configuration, params object[] arguments )
        {
            var customConfig = configuration as CustomConfig;
            if (customConfig == null)
                return;

            output.Write( customConfig.State.Language );
        }
    }
}
