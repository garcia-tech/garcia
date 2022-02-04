namespace GarciaCore.CodeGenerator
{
    public class Generator
    {
        public string InnerGenerate()
        {
            BaseTemplate page = new BaseTemplate();
            string pageContent = page.TransformText();
            //System.IO.File.WriteAllText("outputPage.html", pageContent);
            return pageContent;
        }
    }

    public class EntityGenerator
    {
    }
}