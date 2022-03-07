namespace GarciaCore.CodeGenerator
{
    public class CQRSWebApiControllerGenerator : Generator<CQRSWebApiControllerTemplate>
    {
        public override string DefaultBaseClass => "ApiController";
    }

    public class CQRSWebApiCreateCommandGenerator : Generator<CQRSWebApiCreateCommandTemplate>
    {
        public override string DefaultBaseClass => "IRequest<int>";
    }

    public class CQRSWebApiUpdateCommandGenerator : Generator<CQRSWebApiUpdateCommandTemplate>
    {
        public override string DefaultBaseClass => "IRequest<int>";
    }

    public class CQRSWebApiDeleteCommandGenerator : Generator<CQRSWebApiDeleteCommandTemplate>
    {
        public override string DefaultBaseClass => "IRequest<int>";
    }

    public class CQRSWebApiCreateCommandHandlerGenerator : Generator<CQRSWebApiCreateCommandHandlerTemplate>
    {
        public override string DefaultBaseClass => "";
    }

    public class CQRSWebApiUpdateCommandHandlerGenerator : Generator<CQRSWebApiUpdateCommandHandlerTemplate>
    {
        public override string DefaultBaseClass => "";
    }

    public class CQRSWebApiDeleteCommandHandlerGenerator : Generator<CQRSWebApiDeleteCommandHandlerTemplate>
    {
        public override string DefaultBaseClass => "";
    }

    public class CQRSWebApiQueryGenerator : Generator<CQRSApplicationQueryTemplate>
    {
        public override string DefaultBaseClass => "";
    }
}