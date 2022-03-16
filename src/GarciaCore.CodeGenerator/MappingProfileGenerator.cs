namespace GarciaCore.CodeGenerator
{
    public class MappingProfileGenerator : Generator<MappingProfileTemplate>
    {
        public override string DefaultBaseClass => "Profile";
        protected override string FileNamePostfix => "MappingProfile";
        public override GeneratorType GeneratorType => GeneratorType.MappingProfile;
    }
}