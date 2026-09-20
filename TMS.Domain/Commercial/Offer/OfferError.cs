using TMS.Domain.Shared;
using TMS.Domain.Shared.Results;

namespace TMS.Domain.Commercial.Offer;

public static class OfferError
{
    public static readonly ErrorBase InvalidStatus = ErrorBase.Validation(
        "Offer.InvalidStatus",
        "The offer status is not valid to perform any action");
    public static readonly ErrorBase AlreadyArchived = ErrorBase.Conflict(
        "Offer.AlreadyArchived",
        "The offer is already archived.");
    public static readonly ErrorBase InDraftStatus = ErrorBase.Forbidden(
        "Offer.InDraftStatus",
        "The offer is on the draft status!");
    public static readonly ErrorBase InvalideSellableItem = ErrorBase.Validation(
        "Offer.InvalidSellableItem",
        "The Sellable Item is not valid to continue!");

    public static readonly ErrorBase AlreadyPublished = ErrorBase.Conflict(
        "Offer.AlreadyPublished",
        "The offer is already published");
    public static readonly ErrorBase AlreadyInDraft = ErrorBase.Conflict(
        "Offer.AlreadyInDraft",
        "The offer is already in the draft status.");
    public static readonly ErrorBase InvalidPrice = ErrorBase.Validation(
        "Offer.InvalidPrice",
        "The price of the offer is not valid.");
}
