using FluentValidation;

namespace Wire.Transfer.In.Application.WireTransfers.RegisterTransfer
{
    public class RegisterWireTransferCommandValidator : AbstractValidator<RegisterWireTransferCommand>
    {
        public RegisterWireTransferCommandValidator()
        {
            RuleFor(x => x.Request.Id).NotEmpty();

            RuleFor(x => x.Request.Sender.Name).NotEmpty().MaximumLength(100);

            RuleFor(x => x.Request.Sender.Document.Number).NotEmpty().MaximumLength(14);
            RuleFor(x => x.Request.Sender.Document.Type).NotEmpty()
                .IsEnumName(typeof(DocumentDtoType), false);

            RuleFor(x => x.Request.Sender.Number).NotEmpty().Length(3);
            RuleFor(x => x.Request.Sender.Branch).NotEmpty().Length(4);
            RuleFor(x => x.Request.Sender.Account).NotEmpty().MaximumLength(15);

            RuleFor(x => x.Request.Beneficiary.Name).NotEmpty().MaximumLength(100);

            RuleFor(x => x.Request.Beneficiary.Document.Number).NotEmpty().MaximumLength(14);
            RuleFor(x => x.Request.Beneficiary.Document.Type).NotEmpty()
                .IsEnumName(typeof(DocumentDtoType), false);

            RuleFor(x => x.Request.Beneficiary.Number).NotEmpty().Length(3);
            RuleFor(x => x.Request.Beneficiary.Branch).NotEmpty().Length(4);
            RuleFor(x => x.Request.Beneficiary.Account).NotEmpty().MaximumLength(15);

            RuleFor(x => x.Request.Protocol.Code).NotEmpty().MaximumLength(255);
            RuleFor(x => x.Request.Protocol.Number).NotEmpty();
            RuleFor(x => x.Request.Protocol.Status).NotEmpty().MaximumLength(255);

            RuleFor(x => x.Request.WireTransferType).NotEmpty()
                .IsEnumName(typeof(WireTransferDtoTypeEnum), false);
            RuleFor(x => x.Request.Amount).NotEmpty();
            RuleFor(x => x.Request.TradeDate).NotEmpty();
        }
    }
}