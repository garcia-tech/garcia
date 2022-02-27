namespace GarciaCore.CodeGenerator
{
    public class CQRSWebApiControllerGenerator : Generator<CQRSWebApiControllerTemplate>
    {
        public override string DefaultBaseClass => "ApiController";
    }

    public class CQRSWebApiCommandGenerator : Generator<CQRSWebApiCommandTemplate>
    {
        public override string DefaultBaseClass => "IRequest<bool>";
    }

    public class CQRSWebApiCommandHandlerGenerator : Generator<CQRSWebApiCommandHandlerTemplate>
    {
        public override string DefaultBaseClass => "IRequestHandler<T, bool>";
    }
}