using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TextEditor.Startup))]
namespace TextEditor
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
