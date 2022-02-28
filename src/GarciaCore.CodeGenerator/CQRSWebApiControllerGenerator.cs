namespace GarciaCore.CodeGenerator
{
    public class CQRSWebApiControllerGenerator : Generator<CQRSWebApiControllerTemplate>
    {
        public override string DefaultBaseClass => "ApiController";
    }

    public class CQRSWebApiCommandGenerator : Generator<CQRSWebApiCommandTemplate>
    {
        public override string DefaultBaseClass => "IRequest<int>";
    }

    public class CQRSWebApiCreateCommandHandlerGenerator : Generator<CQRSWebApiCreateCommandHandlerTemplate>
    {
        public override string DefaultBaseClass => "IRequestHandler<T, int>";
    }

    public class CQRSWebApiUpdateCommandHandlerGenerator : Generator<CQRSWebApiUpdateCommandHandlerTemplate>
    {
        public override string DefaultBaseClass => "IRequestHandler<T, int>";
    }

    public class CQRSWebApiDeleteCommandHandlerGenerator : Generator<CQRSWebApiDeleteCommandHandlerTemplate>
    {
        public override string DefaultBaseClass => "IRequestHandler<T, int>";
    }
}