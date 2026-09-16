using System;
using System.Collections.Generic;
using System.Text;
using TMS.Domain.Shared;

namespace TMS.Domain.Learning.LearningPath;

public static class LearningPathError
{
    public static readonly ErrorBase InvalidPathName = ErrorBase.Validation(
        "LearningPath.InvalidPathName",
        "The learning path name is invalid.");

    public static readonly ErrorBase InvalidStatus = ErrorBase.Validation(
        "LearningPath.InvalidStatus",
        "The learning path status is invalid.");

    public static readonly ErrorBase InvalidOutcomes = ErrorBase.Validation(
        "LearningPath.InvalidOutcomes",
        "The learning path outcomes is invalid.");

    public static readonly ErrorBase InvalidAudience = ErrorBase.Validation(
        "LearningPath.InvalidAudience",
        "The learning path audience is invalid.");

    public static readonly ErrorBase AlreadyPublished = ErrorBase.Conflict(
        "LearningPath.AlreadyPublished",
        "The learning path is already published.");

    public static readonly ErrorBase AlreadyArchived = ErrorBase.Conflict(
        "LearningPath.AlreadyArchived",
        "The learning path is already archived.");

    public static readonly ErrorBase CannotArchiveDraft = ErrorBase.Validation(
        "LearningPath.CannotArchiveDraft",
        "The learning path is in the draft status we cannot Archive it.");
}
