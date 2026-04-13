namespace DigiSoft.Common.Global
{
    public static class ResponseConstants
    {
        public const string InvalidInput = "Invalid input";
        public const string InvalidRecaptcha = "reCAPTCHA verification failed";
        public const string Success = "Success";
        public const string AlreadyExists = "Already Exists";
        public const string UserExists = "User Already Exists";
        public const string UserNotExists = "User Does Not Exists";
        public const string InvalidEmail = "Please enter the valid credentials";
        public const string LoginFail = "Invalid Credentials";
        public const string OTPSent = "OTP has been sent successfully";
        public const string ContactAdmin = "Login Failed, Please Contact Admin";
        public const string InvalidOtp = "The OTP is expired or wrong";
        public const string LoginSuccess = "SuccessFully Logged-in";



        public const string InstructorExists = "instructor Already Exists";
        public const string InvalidInstructorId = "Instructor does not exist";
        public const string InstructorCreated = "instructor Created";


        public const string IndustryExists = "Industry Already Exists";
        public const string InvalidIndustryId = "Industry Does Not exist";
        public const string IndustryCreated = "industry created";

        public const string SubIndustryExists = "Sub Industry Already Exists";
        public const string InvalidSubIndustryId = "Sub Industry Does Not exist";
        public const string SubIndustryCreated = "Sub industry created";


        public const string WebsiteExists = "Website Already Exists";
        public const string InvalidWebsiteId = "Website Does Not exist";
        public const string WebsiteCreated = "Website created";


        public const string WebinarExists = "Webinar Already Exists";
        public const string InvalidWebinarId = "Webinar Does Not exist";
        public const string WebinarCreated = "Webinar created";

        public const string ReminderExists = "Reminder already exists";
        public const string ReminderCreated = "Webinar Reminder Created";


        public const string BlogCategoryExists = "Industry Already Exists";
        public const string InvalidBlogCategoryId = "Industry Does Not exist";
        public const string BlogCategoryCreated = "Industry created";


        public const string BlogExists = "Blog Already Exists";
        public const string InvalidBlogId = "Blog Does Not exist";
        public const string BlogCreated = "Blog created";


        public const string EnquiryAdded = "Enquiry Successfully added";

        public const string TestimonialExists = "Testimonial Already Exists";
        public const string InvalidTestimonialId = "Testimonial Does Not exist";
        public const string TestimonialCreated = "Testimonial created";

        public const string NewsLetterSubscriberCreated = "Subscribed Successfully";
        public const string NewsLetterSubscriberDeactivated = "UnSubscribed Successfully";

        public const string TeamExists = "Team Already Exists";
        public const string InvalidTeamId = "Team Does Not exist";
        public const string TeamCreated = "Team created";

        public const string JobApplicationExists = "Job Application Already Exists";
        public const string InvalidJobApplicationId = "Job Application Does Not exist";
        public const string JobApplicationCreated = "Job Application created";

        public const string FaqExists = "Faq Already Exists";
        public const string InvalidFaqId = "Faq Does Not exist";
        public const string FaqCreated = "Faq created";

        public const string MembershipExists = "Membership Already Exists";
        public const string InvalidMembershipId = "Membership Does Not exist";
        public const string MembershipCreated = "Membership created";

        public const string CartEmpty = "Cart is empty";
    }



    public static class S3Directories
    {
        public const string Instructor = "instructor_images";
        public const string Industry = "industry_images";
        public const string Webinar = "webinar_images";
        public const string Blog = "blog_images";
        public const string Team = "team_images";
        public const string User = "user_images";
        public const string Enquiry = "enquiry_images";
        public const string Website = "website_images";
    }


    public static class TemplateName
    {
        public const string Otp = "otp.html";
        public const string Enquiry = "enquiry.html";
        public const string EnquiryThankYou = "enquiry-thank-you.html";
        public const string ForgotPassword = "forgot-password.html";
        public const string LoginTakeover = "login-takeover.html";
        public const string NewWebinarNotification = "new-webinar-notification.html";
        public const string Nda = "nda.html";
    }

    public static class TemplateReplaceStrings
    {
        public const string ReplaceOTP = "{OTP}";
        public const string UserName = "{userName}";
        public const string Password = "{password}";

        public const string Name = "{{Name}}";
        public const string Email = "{{Email}}";
        public const string PhoneNumber = "{{PhoneNumber}}";
        public const string Subject = "{{Subject}}";
        public const string Message = "{{Message}}";
        public const string Date = "{{Date}}";
        public const string Year = "{{Year}}";
        public const string CompanyName = "{{CompanyName}}";
        public const string WebsiteURL = "{{WebsiteURL}}";
        public const string MonthlyMarketingBudget = "{{MonthlyMarketingBudget}}";
        public const string Media = "{{Media}}";

        public const string ReplaceURL = "{URL}";

        public const string WebinarName = "{WebinarName}";
        public const string WebinarDate = "{WebinarDate}";
        public const string WebinarTime = "{WebinarTime}";
        public const string Duration = "{Duration}";
        public const string WebinarOverview = "{WebinarOverView}";
        public const string WhyShouldYouAttend = "{WhyShouldYouAttend}";
        public const string WhoWillBenefit = "{WhoWillBenefit}";
        public const string RegistrationUrl = "{RegistrationUrl}";
        public const string LogoUrl = "{LogoUrl}";
    }

    public static class EmailSubject
    {
        public const string OtpSubject = "OTP to login";
        public const string EnquirySubject = "New Enquiry Received";
        public const string EnquiryThankYouSubject = "Thank you for your enquiry";
        public const string NdaSubject = "Non-Disclosure Agreement (NDA)";
    }
}
