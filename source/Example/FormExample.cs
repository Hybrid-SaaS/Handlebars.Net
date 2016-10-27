using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Example
{
    public partial class FormExample : Form
    {
        public FormExample()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBoxLanguage.SelectedIndex = 0;
        }

        private void buttonRun_Click(object sender, EventArgs e)
        {
            if ( !checkBoxUseCache.Checked )
            {
                //no cache, empty dictionary
                TemplateCache.PartialCache = null;
            }

            //this is the state I want to track: The pulldown value
            var language = comboBoxLanguage.SelectedItem.ToString();

            //I create a new Handlebars instance
            var handlebars = TemplateCache.GetHandleBars(new State
            {
                Language = language
            });

            var compiled = handlebars.Compile(@"
<div style='margin: 1px; border: solid 1px silver; padding: 2px; font: 10pt Tahoma'>
    Current language: {{language}}
</div>

{{ >partial1 }}
{{ >partial2 }}
{{ >partial3 }}
");
            var data = new Dictionary<string, object>
            {
                ["language"] = language
            };

            var result = compiled( data );

            webBrowser.DocumentText = result;
        }
    }
}
