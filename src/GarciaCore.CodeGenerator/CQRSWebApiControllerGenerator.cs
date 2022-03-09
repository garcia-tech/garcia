namespace GarciaCore.CodeGenerator
{
    public class CQRSWebApiControllerGenerator : Generator<CQRSWebApiControllerTemplate>
    {
        public override string DefaultBaseClass => "ApiController";
    }

    public class CQRSApplicationCreateCommandGenerator : Generator<CQRSApplicationCreateCommandTemplate>
    {
        public override string DefaultBaseClass => "IRequest<int>";
    }

    public class CQRSApplicationUpdateCommandGenerator : Generator<CQRSApplicationUpdateCommandTemplate>
    {
        public override string DefaultBaseClass => "IRequest<int>";
    }

    public class CQRSApplicationDeleteCommandGenerator : Generator<CQRSApplicationDeleteCommandTemplate>
    {
        public override string DefaultBaseClass => "IRequest<int>";
    }

    public class CQRSApplicationCreateCommandHandlerGenerator : Generator<CQRSApplicationCreateCommandHandlerTemplate>
    {
        public override string DefaultBaseClass => "";
    }

    public class CQRSApplicationUpdateCommandHandlerGenerator : Generator<CQRSApplicationUpdateCommandHandlerTemplate>
    {
        public override string DefaultBaseClass => "";
    }

    public class CQRSApplicationDeleteCommandHandlerGenerator : Generator<CQRSApplicationDeleteCommandHandlerTemplate>
    {
        public override string DefaultBaseClass => "";
    }

    public class CQRSWebApiQueryGenerator : Generator<CQRSApplicationQueryTemplate>
    {
        public override string DefaultBaseClass => "";
    }
}