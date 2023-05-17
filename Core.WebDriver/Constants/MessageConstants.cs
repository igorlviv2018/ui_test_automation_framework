namespace Taf.UI.Core.Constants
{
    public class MessageConstants
    {
        public const string PasswordShouldBeSpecificLong  = "Password should be 8-64 characters long and should contain both letters and digits.";

        public const string ConfirmPassNotEqualToNewPass  = "Confirmation password is not equal to new password";

        public const string IncorectCurrentPassword  = "Current password is incorrect";

        // Authoring: operation (Archive, Duplicate, Restore article etc) confirm modal
        public const string AuthoringConfirmModalMessage = "Are you sure you want to {0} {1} article{2}?";

        public const string AuthoringConfirmOperationModalMessage = "Are you sure you want to {0} {1} {2}{3}?";

        public const string TafNewsCaption = "New {0} is available";

        public const string DeleteParameterModalTitle = "Delete Parameter?";

        public const string DeleteParameterModalMessage = "You will not be able to recover this Parameter.";

        public const string NoMatchingDevicesText = "No matching devices found by your request. Please change the requirements.";

        public const string ProfileUpdateSuccessMessage = "Profile changes were saved";

        public const string UserWithSuchEmailNotFoundMessage = "User with such email ({0}) is not found";
    }
}
