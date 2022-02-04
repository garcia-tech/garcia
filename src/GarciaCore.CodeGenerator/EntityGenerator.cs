namespace GarciaCore.CodeGenerator
{
    public class Generator
    {
        public string InnerGenerate()
        {
            BaseTemplate page = new BaseTemplate();
            String pageContent = page.TransformText();
            System.IO.File.WriteAllText("outputPage.html", pageContent);
        }
    }

    public class EntityGenerator
    {
    }
}