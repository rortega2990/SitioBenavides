namespace BenavidesFarm.DataModels.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable("dbo.Estados", s => new
            {
                Id = s.Int(nullable: false, identity: true),
                Name = s.String(nullable: false),
                Lng = s.String(),
                Lat = s.String()
            })
            .PrimaryKey(s => s.Id);

            CreateTable("dbo.Municipios", c => new
            {
                Id = c.Int(nullable: false, identity: true),
                Name = c.String(nullable: false),
                Lng = c.String(),
                Lat = c.String()
            })
            .PrimaryKey(c => c.Id);

            CreateTable("dbo.EstadosMunicipios", stc => new
            {
                Id = stc.Int(nullable: false, identity: true),
                Estado_Id = stc.Int(),
                Municipio_Id = stc.Int()
            })
            .PrimaryKey(stc => stc.Id)
            .ForeignKey("dbo.Estados", stc => stc.Estado_Id)
            .ForeignKey("dbo.Municipios", stc => stc.Municipio_Id)
            .Index(stc => stc.Estado_Id)
            .Index(stc => stc.Municipio_Id);

            CreateTable(
                "dbo.BenefitSections",
                c => new
                    {
                        BenefitSectionId = c.Int(nullable: false, identity: true),
                        BenefitSectionTitle = c.String(),
                        BenefitSectionDiv = c.String(),
                        BenefitSectionParagraph = c.String(),
                        BenefitSectionImage = c.String(),
                        BenefitSectionImageXS = c.String(),
                        BenefitSectionCustomValue = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.BenefitSectionId);
            
            CreateTable(
                "dbo.BillingPages",
                c => new
                    {
                        BillingPageId = c.Int(nullable: false, identity: true),
                        BillingPageTitle = c.String(nullable: false),
                        BenefitSectionId = c.Int(nullable: false),
                        IncrementBenefitSectionId = c.Int(nullable: false),
                        LabSectionId = c.Int(nullable: false),
                        BillingPageActive = c.Boolean(nullable: false),
                        BillingPageCreatedDate = c.DateTime(nullable: false),
                        BillingPageCustomValue = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.BillingPageId)
                .ForeignKey("dbo.BenefitSections", t => t.BenefitSectionId, cascadeDelete: true)
                .ForeignKey("dbo.IncrementBenefitSections", t => t.IncrementBenefitSectionId, cascadeDelete: true)
                .ForeignKey("dbo.LabSections", t => t.LabSectionId, cascadeDelete: true)
                .Index(t => t.BenefitSectionId)
                .Index(t => t.IncrementBenefitSectionId)
                .Index(t => t.LabSectionId);
            
            CreateTable(
                "dbo.IncrementBenefitSections",
                c => new
                    {
                        IncrementBenefitSectionId = c.Int(nullable: false, identity: true),
                        IncrementBenefitSectionTitle = c.String(),
                        IncrementBenefitSectionDiv = c.String(),
                        IncrementBenefitSectionImage1 = c.String(),
                        IncrementBenefitSectionImage2 = c.String(),
                        IncrementBenefitSectionCustomValue = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.IncrementBenefitSectionId);
            
            CreateTable(
                "dbo.LabSections",
                c => new
                    {
                        LabSectionId = c.Int(nullable: false, identity: true),
                        LabSectionTitle = c.String(nullable: false),
                        LabSectionCustomValue = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.LabSectionId);
            
            CreateTable(
                "dbo.BlogPages",
                c => new
                    {
                        BlogPageId = c.Int(nullable: false, identity: true),
                        BlogPageTitle = c.String(nullable: false),
                        BlogTypeId = c.Int(nullable: false),
                        BlogPageActive = c.String(nullable: false, maxLength: 15),
                        BlogPageCreatedDate = c.DateTime(nullable: false),
                        BlogPageCustomValue = c.String(nullable: false),
                        BlogPageColorBgHead = c.String(),
                        BlogPageColorTextHead = c.String(),
                        BlogPageTextHead = c.String(),
                        BlogPageColorTextDescHead = c.String(),
                        BlogPageTextDesc = c.String(),
                        BlogPageTitleDesc = c.String(),
                        BlogPageColorTitleDesc = c.String(),
                        BlogPageColorBgTitleDesc = c.String(),
                        BlogPageImage = c.String(),
                    })
                .PrimaryKey(t => t.BlogPageId)
                .ForeignKey("dbo.BlogTypes", t => t.BlogTypeId, cascadeDelete: true)
                .Index(t => t.BlogTypeId);
            
            CreateTable(
                "dbo.BlogTypes",
                c => new
                    {
                        BlogTypeId = c.Int(nullable: false, identity: true),
                        BlogTypeName = c.String(),
                        BlogTypeActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.BlogTypeId);
            
            CreateTable(
                "dbo.NewsPages",
                c => new
                    {
                        NewsPageId = c.Int(nullable: false, identity: true),
                        NewsPageTitle = c.String(nullable: false),
                        BlogPageId = c.Int(nullable: false),
                        NewsPageActive = c.String(nullable: false),
                        NewsPageCreatedDate = c.DateTime(nullable: false),
                        NewsPageCustomValue = c.String(nullable: false),
                        NewsPageColorBgHead = c.String(),
                        NewsPageTextHead = c.String(),
                        NewsPageColorTextHead = c.String(),
                        NewsPageSubTextHead = c.String(),
                        NewsPageColorSubTextHead = c.String(),
                        NewsPageColorBgSubTextHead = c.String(),
                        NewsPageImageHead = c.String(),
                        NewsPageImageDescription = c.String(),
                        NewsPageTitleDescription1 = c.String(),
                        NewsPageTitleDescription2 = c.String(),
                        NewsPageTextDescription1 = c.String(),
                        NewsPageTextDescription2 = c.String(),
                        NewsPageColorTitleDescription1 = c.String(),
                        NewsPageColorTitleDescription2 = c.String(),
                        NewsPageUrl = c.String(),
                        NewsPageOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.NewsPageId)
                .ForeignKey("dbo.BlogPages", t => t.BlogPageId, cascadeDelete: true)
                .Index(t => t.BlogPageId);
            
            CreateTable(
                "dbo.BlogSections",
                c => new
                    {
                        BlogSectionId = c.Int(nullable: false, identity: true),
                        BlogSectionTitle = c.String(),
                        BlogSectionColorTitle = c.String(),
                        BlogSectionCustomValue = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.BlogSectionId);
            
            CreateTable(
                "dbo.HomePages",
                c => new
                    {
                        HomePageId = c.Int(nullable: false, identity: true),
                        HomePageTitle = c.String(nullable: false),
                        OfferSectionId = c.Int(nullable: false),
                        CardSectionId = c.Int(nullable: false),
                        FoseSectionId = c.Int(nullable: false),
                        BlogSectionId = c.Int(nullable: false),
                        HomePageActive = c.Boolean(nullable: false),
                        HomePageCreatedDate = c.DateTime(nullable: false),
                        HomePageCustomValue = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.HomePageId)
                .ForeignKey("dbo.BlogSections", t => t.BlogSectionId, cascadeDelete: true)
                .ForeignKey("dbo.CardSections", t => t.CardSectionId, cascadeDelete: true)
                .ForeignKey("dbo.FoseSections", t => t.FoseSectionId, cascadeDelete: true)
                .ForeignKey("dbo.OfferSections", t => t.OfferSectionId, cascadeDelete: true)
                .Index(t => t.OfferSectionId)
                .Index(t => t.CardSectionId)
                .Index(t => t.FoseSectionId)
                .Index(t => t.BlogSectionId);
            
            CreateTable(
                "dbo.CardSections",
                c => new
                    {
                        CardSectionId = c.Int(nullable: false, identity: true),
                        CardSectionTitle = c.String(nullable: false),
                        CardSectionColorTitle = c.String(),
                        CardSectionUrl = c.String(),
                        CardSectionImage = c.String(),
                        CardSectionImageXS = c.String(),
                        CardSectionCustomValue = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.CardSectionId);
            
            CreateTable(
                "dbo.FoseSections",
                c => new
                    {
                        FoseSectionId = c.Int(nullable: false, identity: true),
                        FoseSectionTitle = c.String(nullable: false),
                        FoseSectionColorTitle = c.String(maxLength: 10),
                        FoseSectionWord1 = c.String(),
                        FoseSectionColorWord1 = c.String(maxLength: 10),
                        FoseSectionWord2 = c.String(),
                        FoseSectionColorWord2 = c.String(maxLength: 10),
                        FoseSectionImage = c.String(maxLength: 1024),
                        FoseSectionImageLogo = c.String(maxLength: 1024),
                        FoseSectionCustomValue = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.FoseSectionId);
            
            CreateTable(
                "dbo.OfferSections",
                c => new
                    {
                        OfferSectionId = c.Int(nullable: false, identity: true),
                        OfferSectionTitle = c.String(),
                        OfferSectionColorTitle = c.String(),
                        OfferSectionCustomValue = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.OfferSectionId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductId = c.Int(nullable: false, identity: true),
                        ProductName = c.String(maxLength: 100),
                        ProductTitle_TitleTypeId = c.Int(),
                        ProductImage = c.String(),
                        ProductOrder = c.Int(nullable: false),
                        ProductURL = c.String(),
                        ProductCustomValue = c.String(nullable: false),
                        ProductSubtitle = c.String(),
                        BlogSectionId = c.Int(),
                        OfferSectionId = c.Int(),
                        ServiceTypeId = c.Int(),
                        OfferTypeId = c.Int(),
                        BlogTypeId = c.Int(),
                    })
                .PrimaryKey(t => t.ProductId)
                .ForeignKey("dbo.BlogSections", t => t.BlogSectionId)
                .ForeignKey("dbo.BlogTypes", t => t.BlogTypeId)
                .ForeignKey("dbo.OfferSections", t => t.OfferSectionId)
                .ForeignKey("dbo.OfferTypes", t => t.OfferTypeId)
                .ForeignKey("dbo.TitleTypes", t => t.ProductTitle_TitleTypeId)
                .ForeignKey("dbo.ServiceTypes", t => t.ServiceTypeId)
                .Index(t => t.ProductName, unique: true)
                .Index(t => t.ProductTitle_TitleTypeId)
                .Index(t => t.BlogSectionId)
                .Index(t => t.OfferSectionId)
                .Index(t => t.ServiceTypeId)
                .Index(t => t.OfferTypeId)
                .Index(t => t.BlogTypeId);
            
            CreateTable(
                "dbo.OfferTypes",
                c => new
                    {
                        OfferTypeId = c.Int(nullable: false, identity: true),
                        OfferTypeName = c.String(),
                        OfferTypeActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.OfferTypeId);
            
            CreateTable(
                "dbo.ProductPages",
                c => new
                    {
                        ProductPageId = c.Int(nullable: false, identity: true),
                        ProductPageActive = c.String(nullable: false),
                        ProductPageCreatedDate = c.DateTime(nullable: false),
                        ProductPageCustomValue = c.String(nullable: false),
                        PromocionPageId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        ProductPageBgColor = c.String(),
                        ProductPageTextTitle = c.String(),
                        ProductPageColorTextTitle = c.String(),
                        ProductPageTextDescription1 = c.String(),
                        ProductPageTextDescription2 = c.String(),
                        ProductPageTextCharacteristic1 = c.String(),
                        ProductPageTextCharacteristic2 = c.String(),
                        ProductPageColorTextDescription1 = c.String(),
                        ProductPageColorTextDescription2 = c.String(),
                        ProductPageColorTextCharacteristic1 = c.String(),
                        ProductPageColorTextCharacteristic2 = c.String(),
                    })
                .PrimaryKey(t => t.ProductPageId)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.PromocionPages", t => t.PromocionPageId, cascadeDelete: true)
                .Index(t => t.PromocionPageId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.PromocionPages",
                c => new
                    {
                        PromocionPageId = c.Int(nullable: false, identity: true),
                        PromocionPageOrder = c.Int(nullable: false),
                        FosePageId = c.Int(nullable: false),
                        PromocionPageCustomValue = c.String(nullable: false),
                        PromocionPageImageLogo1 = c.String(),
                        PromocionPageImageLogo2 = c.String(),
                        PromocionPageActive = c.String(nullable: false),
                        PromocionPageCreatedDate = c.DateTime(nullable: false),
                        PromocionPageHeadText = c.String(),
                        PromocionPageSpanHeadText = c.String(),
                        PromocionPageColorHeadBg = c.String(),
                        PromocionPageHeadtextColor = c.String(),
                        PromocionPageSpanHeadtextColor = c.String(),
                        PromocionPageSubText1 = c.String(),
                        PromocionPageSubText2 = c.String(),
                        PromocionPageHeadImage = c.String(),
                        PromocionPageTextFose = c.String(),
                        PromocionPageTextColorFose = c.String(),
                    })
                .PrimaryKey(t => t.PromocionPageId)
                .ForeignKey("dbo.FosePages", t => t.FosePageId, cascadeDelete: true)
                .Index(t => t.FosePageId);
            
            CreateTable(
                "dbo.FosePages",
                c => new
                    {
                        FosePageId = c.Int(nullable: false, identity: true),
                        FosePageTitle = c.String(),
                        FoseTextBranch = c.String(),
                        FosePageActive = c.String(nullable: false, maxLength: 15),
                        FosePageCreatedDate = c.DateTime(nullable: false),
                        FosePageCustomValue = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.FosePageId);
            
            CreateTable(
                "dbo.TitleTypes",
                c => new
                    {
                        TitleTypeId = c.Int(nullable: false, identity: true),
                        TitleTypeMessage = c.String(),
                        TitleTypeColor = c.String(),
                        TitleTypeHoverColor = c.String(),
                        TitleTypeBgColor = c.String(),
                        TitleTypeSpan = c.String(),
                        TitleTypeCustomValue = c.String(),
                    })
                .PrimaryKey(t => t.TitleTypeId);
            
            CreateTable(
                "dbo.ServiceTypes",
                c => new
                    {
                        ServiceTypeId = c.Int(nullable: false, identity: true),
                        ServiceTypeName = c.String(),
                        ServiceTypeNameDescription = c.String(),
                        ServiceTypeProdutcsDescription = c.String(),
                        ServiceTypeActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ServiceTypeId);
            
            CreateTable(
                "dbo.BranchPages",
                c => new
                    {
                        BranchPageId = c.Int(nullable: false, identity: true),
                        BranchPageTitle = c.String(nullable: false),
                        BranchPageMessage = c.String(),
                        BranchPageBranchNames = c.String(),
                        BranchPageActive = c.Boolean(nullable: false),
                        BranchPageCreatedDate = c.DateTime(nullable: false),
                        BranchPageCustomValue = c.String(nullable: false),
                        BranchPageColorMessage = c.String(),
                        BranchPageColorTextBranchNames = c.String(),

                    })
                .PrimaryKey(t => t.BranchPageId);

            CreateTable(
                "dbo.Branches",
                c => new
                {
                    BranchId = c.Int(nullable: false, identity: true),
                    BranchName = c.String(),
                    BranchRegion = c.String(),
                    BranchCeco = c.String(),
                    BranchSap = c.String(),
                    BranchAddress = c.String(),
                    BranchCity = c.String(),
                    BranchConsult = c.Boolean(nullable: false),
                    BranchHour1 = c.String(),
                    BranchHour2 = c.String(),
                    BranchLongitude = c.String(),
                    BranchLatitude = c.String(),
                    BranchActive = c.Boolean(nullable: false),
                    City_Id = c.Int(nullable:true),
                    State_Id = c.Int(nullable:true)
                    })
                .PrimaryKey(t => t.BranchId)
                .ForeignKey("dbo.Municipios", b => b.City_Id)
                .ForeignKey("dbo.Estados", b => b.State_Id)
                .Index(b => b.State_Id)
                .Index(b => b.City_Id);
            
            CreateTable(
                "dbo.ConditionsTermsPages",
                c => new
                    {
                        ConditionsTermsPageId = c.Int(nullable: false, identity: true),
                        ConditionsTermsPageTitle = c.String(nullable: false),
                        ConditionsTermsPageActive = c.Boolean(nullable: false),
                        ConditionsTermsPageCreatedDate = c.DateTime(nullable: false),
                        ConditionsTermsPageCustomValue = c.String(nullable: false),
                        ConditionsTermsPageHeadText = c.String(),
                        ConditionsTermsPageColorHeadText = c.String(),
                        ConditionsTermsPageBgColorHead = c.String(),
                        ConditionsTermsPageTextDescription = c.String(),
                        ConditionsTermsPageTextColor = c.String(),
                        ConditionsTermsPageTextTitle = c.String(),

                    })
                .PrimaryKey(t => t.ConditionsTermsPageId);
            
            CreateTable(
                "dbo.ContactPages",
                c => new
                    {
                        ContactPageId = c.Int(nullable: false, identity: true),
                        ContactPageTitle = c.String(nullable: false),
                        ContactPageActive = c.Boolean(nullable: false),
                        ContactPageCreatedDate = c.DateTime(nullable: false),
                        ContactPageCustomValue = c.String(nullable: false),
                        ContactPageHeadText = c.String(),
                        ContactPageColorHeadText = c.String(),
                        ContactPageBgColorHead = c.String(),
                        ContactPageSubText1 = c.String(),
                        ContactPageSubText2 = c.String(),
                        ContactPageColorSubText1 = c.String(),
                        ContactPageColorSubText2 = c.String(),
                        ContactPageAddress = c.String(),
                        ContactPageTelAddress = c.String(),
                        ContactPageTelSaD = c.String(),
                        ContactPageEmailSaD = c.String(),
                        ContactPageTelAaP = c.String(),
                        ContactPageColorTextFooter = c.String(),
                    })
                .PrimaryKey(t => t.ContactPageId);
            
            CreateTable(
                "dbo.ContactUsers",
                c => new
                    {
                        ContactUserId = c.Int(nullable: false, identity: true),
                        Names = c.String(nullable: false),
                        PhoneNumber = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Suggests = c.String(),
                        DateCreation = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ContactUserId);
            
            CreateTable(
                "dbo.DocumentFiles",
                c => new
                    {
                        DocumentFilesId = c.Int(nullable: false, identity: true),
                        DocumentTypeId = c.Int(nullable: false),
                        NameDescriptiveFile = c.String(),
                        AddressFile = c.String(),
                    })
                .PrimaryKey(t => t.DocumentFilesId)
                .ForeignKey("dbo.DocumentTypes", t => t.DocumentTypeId, cascadeDelete: true)
                .Index(t => t.DocumentTypeId);
            
            CreateTable(
                "dbo.DocumentTypes",
                c => new
                    {
                        DocumentTypeId = c.Int(nullable: false, identity: true),
                        DocumentName = c.String(),
                        DocumentDescription = c.String(),
                        DocumentActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.DocumentTypeId);
            
            CreateTable(
                "dbo.ImageSections",
                c => new
                    {
                        ImageSectionId = c.Int(nullable: false, identity: true),
                        ImageSectionImage = c.String(maxLength: 1024),
                        ImageSectionPageId = c.Int(nullable: false),
                        ImageSectionPageName = c.String(nullable: false, maxLength: 100),
                        ImageSectionActive = c.Boolean(nullable: false),
                        ImageSectionText = c.String(),
                        ImageSectionColorText = c.String(),
                    })
                .PrimaryKey(t => t.ImageSectionId)
                .Index(t => t.ImageSectionPageId)
                .Index(t => t.ImageSectionPageName);
            
            CreateTable(
                "dbo.InterestAreas",
                c => new
                    {
                        InterestAreaId = c.Int(nullable: false, identity: true),
                        InterestAreaName = c.String(),
                        InterestAreaActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.InterestAreaId);
            
            CreateTable(
                "dbo.InvestorPages",
                c => new
                    {
                        InvestorPageId = c.Int(nullable: false, identity: true),
                        InvestorPageTitle = c.String(nullable: false),
                        InvestorPageActive = c.Boolean(nullable: false),
                        InvestorPageCreatedDate = c.DateTime(nullable: false),
                        InvestorPageCustomValue = c.String(nullable: false),
                        InvestorPageHeadText = c.String(),
                        InvestorPageColorHeadText = c.String(),
                        InvestorPageColorHeadBg = c.String(),
                        InvestorPageSubText = c.String(),
                        InvestorPageColorSubText = c.String(),
                    })
                .PrimaryKey(t => t.InvestorPageId);
            
            CreateTable(
                "dbo.JoinTeamPages",
                c => new
                    {
                        JoinTeamPageId = c.Int(nullable: false, identity: true),
                        JoinTeamPageTitle = c.String(nullable: false),
                        JoinTeamPageActive = c.Boolean(nullable: false),
                        JoinTeamPageCreatedDate = c.DateTime(nullable: false),
                        JoinTeamPageCustomValue = c.String(nullable: false),
                        JoinTeamPageSubText1 = c.String(),
                        JoinTeamPageSubText2 = c.String(),
                        JoinTeamPageColorText1 = c.String(),
                        JoinTeamPageColorText2 = c.String(),
                    })
                .PrimaryKey(t => t.JoinTeamPageId);
            
            CreateTable(
                "dbo.OfferPages",
                c => new
                    {
                        OfferPageId = c.Int(nullable: false, identity: true),
                        OfferPageTitle = c.String(nullable: false),
                        OfferTypeId = c.Int(nullable: false),
                        OfferPageActive = c.Boolean(nullable: false),
                        OfferPageCreatedDate = c.DateTime(nullable: false),
                        OfferPageCustomValue = c.String(nullable: false),
                        OfferPageText1 = c.String(),
                        OfferPageColorText1 = c.String(),
                        OfferPageSpan1 = c.String(),
                        OfferPageColorSpan1 = c.String(),
                        OfferPageTextType1 = c.String(),
                        OfferPageText2 = c.String(),
                        OfferPageColorText2 = c.String(),
                        OfferPageSpan2 = c.String(),
                        OfferPageColorSpan2 = c.String(),
                        OfferPageTextType2 = c.String(),
                        OfferPageText3 = c.String(),
                        OfferPageColorText3 = c.String(),
                        OfferPageSpan3 = c.String(),
                        OfferPageColorSpan3 = c.String(),
                        OfferPageTextType3 = c.String(),
                        OfferPageFillColor = c.String(),
                        OfferImage = c.String(),
                    })
                .PrimaryKey(t => t.OfferPageId)
                .ForeignKey("dbo.OfferTypes", t => t.OfferTypeId, cascadeDelete: true)
                .Index(t => t.OfferTypeId);
            
            CreateTable(
                "dbo.PagePreviews",
                c => new
                    {
                        PageName = c.String(nullable: false, maxLength: 128),
                        PageValue = c.Binary(),
                    })
                .PrimaryKey(t => t.PageName);
            
            CreateTable(
                "dbo.PrivacityPages",
                c => new
                    {
                        PrivacityPageId = c.Int(nullable: false, identity: true),
                        PrivacityPageTitle = c.String(nullable: false),
                        PrivacityPageActive = c.Boolean(nullable: false),
                        PrivacityPageCreatedDate = c.DateTime(nullable: false),
                        PrivacityPageCustomValue = c.String(nullable: false),
                        PrivacityPageHeadText = c.String(),
                        PrivacityPageColorHeadText = c.String(),
                        PrivacityPageBgColorHead = c.String(),
                        PrivacityPageTextDescription = c.String(),
                        PrivacityPageTextColor = c.String(),
                        PrivacityPageTextTitle = c.String(),
                    })
                .PrimaryKey(t => t.PrivacityPageId);
            
            CreateTable(
                "dbo.ProviderPages",
                c => new
                    {
                        ProviderPageId = c.Int(nullable: false, identity: true),
                        ProviderPageTitle = c.String(nullable: false),
                        ProviderPageActive = c.Boolean(nullable: false),
                        ProviderPageCreatedDate = c.DateTime(nullable: false),
                        ProviderPageCustomValue = c.String(nullable: false),
                        ProviderPageHeadText = c.String(),
                        ProviderPageColorHeadText = c.String(),
                        ProviderPageBgColorHead = c.String(),
                        ProviderPageSubText = c.String(),
                        ProviderPageColorSubText = c.String(),
                    })
                .PrimaryKey(t => t.ProviderPageId);
            
            CreateTable(
                "dbo.ReportFiles",
                c => new
                    {
                        ReportFilesId = c.Int(nullable: false, identity: true),
                        ReportTypeId = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
                        AddressFile = c.String(),
                        DescriptionFile = c.String(),
                    })
                .PrimaryKey(t => t.ReportFilesId)
                .ForeignKey("dbo.ReportTypes", t => t.ReportTypeId, cascadeDelete: true)
                .Index(t => t.ReportTypeId);
            
            CreateTable(
                "dbo.ReportTypes",
                c => new
                    {
                        ReportTypeId = c.Int(nullable: false, identity: true),
                        ReportName = c.String(),
                        ReportDescription = c.String(),
                        ReportActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ReportTypeId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.BenUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.SaDPages",
                c => new
                    {
                        SaDPageId = c.Int(nullable: false, identity: true),
                        SaDPageTitle = c.String(nullable: false),
                        SaDPageActive = c.Boolean(nullable: false),
                        SaDPageCreatedDate = c.DateTime(nullable: false),
                        SaDPageCustomValue = c.String(nullable: false),
                        SaDPageHeadText1 = c.String(),
                        SaDPageHeadText2 = c.String(),
                        SaDPageHeadTextColor1 = c.String(),
                        SaDPageHeadTextColor2 = c.String(),
                        SaDPageImageBg = c.String(),
                        SaDPageImageLogo = c.String(),
                        SaDPageSubTextColor1 = c.String(),
                        SaDPageSubText1 = c.String(),
                        SaDPageNumberPrincipalText = c.String(),
                        SaDPageNumberPrincipalTextColor = c.String(),
                        SaDPageNumberPrincipalBgColor = c.String(),
                    })
                .PrimaryKey(t => t.SaDPageId);
            
            CreateTable(
                "dbo.SaDTypeNumbers",
                c => new
                    {
                        SaDTypeNumberId = c.Int(nullable: false, identity: true),
                        SaDTypeNumberCity = c.String(),
                        SaDTypeNumberPhone = c.String(),
                        SaDTypeNumberActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.SaDTypeNumberId);
            
            CreateTable(
                "dbo.ServicePages",
                c => new
                    {
                        ServicePageId = c.Int(nullable: false, identity: true),
                        ServicePageTitle = c.String(nullable: false),
                        ServicePageActive = c.Boolean(nullable: false),
                        ServicePageCreatedDate = c.DateTime(nullable: false),
                        ServicePageCustomValue = c.String(nullable: false),
                        ServicePageImageLogo = c.String(),
                        ServicePageHeadText1 = c.String(),
                        ServicePageHeadSubText1 = c.String(),
                        ServicePageColorHeadText1 = c.String(),
                        ServicePageColorHeadSubText1 = c.String(),
                        ServicePageColorHeadBg = c.String(),
                        ServicePageSubText = c.String(),
                        ServicePageSubTextDescription = c.String(),
                        ServicePageColorSubText = c.String(),
                        ServicePageColorSubTextDescription = c.String(),
                    })
                .PrimaryKey(t => t.ServicePageId);
            
            CreateTable(
                "dbo.UserJoinedToTeams",
                c => new
                    {
                        UserJoinedToTeamId = c.Int(nullable: false, identity: true),
                        Names = c.String(nullable: false),
                        Lastnames = c.String(nullable: false),
                        PhoneNumber = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        InterestArea = c.String(),
                        Address = c.String(),
                        DateCreation = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserJoinedToTeamId);
            
            CreateTable(
                "dbo.UserProfileInfos",
                c => new
                    {
                        UserProfileInfoId = c.Int(nullable: false, identity: true),
                        UserNames = c.String(nullable: false),
                        UserLastName1 = c.String(nullable: false),
                        UserLastName2 = c.String(nullable: false),
                        UserBirthDate = c.DateTime(nullable: false),
                        UserFemale = c.Boolean(nullable: false),
                        UserHasChildren = c.Boolean(nullable: false),
                        UserClubPeques = c.Boolean(),
                        UserImagePerfil = c.String(),
                        UserMount = c.Single(nullable: false),
                        UserCreationDateClubPeques = c.DateTime(),
                        UserCreationDate = c.DateTime(nullable: false),
                        UserUpdateDate = c.DateTime(nullable: false),
                        UserCodePostal = c.String(),
                        UserCity = c.String(),
                    })
                .PrimaryKey(t => t.UserProfileInfoId);
            
            CreateTable(
                "dbo.BenUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CardUser = c.String(maxLength: 50),
                        TypeUser = c.String(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        UserProfileInfo_UserProfileInfoId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfileInfos", t => t.UserProfileInfo_UserProfileInfoId)
                .Index(t => t.CardUser, unique: true)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.UserProfileInfo_UserProfileInfoId);
            
            CreateTable(
                "dbo.UserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BenUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.BenUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BenUsers", "UserProfileInfo_UserProfileInfoId", "dbo.UserProfileInfos");
            DropForeignKey("dbo.UserRoles", "UserId", "dbo.BenUsers");
            DropForeignKey("dbo.UserLogins", "UserId", "dbo.BenUsers");
            DropForeignKey("dbo.UserClaims", "UserId", "dbo.BenUsers");
            DropForeignKey("dbo.UserRoles", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.ReportFiles", "ReportTypeId", "dbo.ReportTypes");
            DropForeignKey("dbo.OfferPages", "OfferTypeId", "dbo.OfferTypes");
            DropForeignKey("dbo.DocumentFiles", "DocumentTypeId", "dbo.DocumentTypes");
            DropForeignKey("dbo.Products", "ServiceTypeId", "dbo.ServiceTypes");
            DropForeignKey("dbo.Products", "ProductTitle_TitleTypeId", "dbo.TitleTypes");
            DropForeignKey("dbo.ProductPages", "PromocionPageId", "dbo.PromocionPages");
            DropForeignKey("dbo.PromocionPages", "FosePageId", "dbo.FosePages");
            DropForeignKey("dbo.ProductPages", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Products", "OfferTypeId", "dbo.OfferTypes");
            DropForeignKey("dbo.Products", "OfferSectionId", "dbo.OfferSections");
            DropForeignKey("dbo.Products", "BlogTypeId", "dbo.BlogTypes");
            DropForeignKey("dbo.Products", "BlogSectionId", "dbo.BlogSections");
            DropForeignKey("dbo.HomePages", "OfferSectionId", "dbo.OfferSections");
            DropForeignKey("dbo.HomePages", "FoseSectionId", "dbo.FoseSections");
            DropForeignKey("dbo.HomePages", "CardSectionId", "dbo.CardSections");
            DropForeignKey("dbo.HomePages", "BlogSectionId", "dbo.BlogSections");
            DropForeignKey("dbo.NewsPages", "BlogPageId", "dbo.BlogPages");
            DropForeignKey("dbo.BlogPages", "BlogTypeId", "dbo.BlogTypes");
            DropForeignKey("dbo.BillingPages", "LabSectionId", "dbo.LabSections");
            DropForeignKey("dbo.BillingPages", "IncrementBenefitSectionId", "dbo.IncrementBenefitSections");
            DropForeignKey("dbo.BillingPages", "BenefitSectionId", "dbo.BenefitSections");
            DropForeignKey("dbo.Branches", "State_Id", "dbo.Estados");
            DropForeignKey("dbo.Branches", "City_Id", "dbo.Municipios");
            DropForeignKey("dbo.EstadosMunicipios", "Estado_Id", "dbo.Estados");
            DropForeignKey("dbo.EstadosMunicipios", "Municipio_Id", "dbo.Municipios");
            DropIndex("dbo.UserLogins", new[] { "UserId" });
            DropIndex("dbo.UserClaims", new[] { "UserId" });
            DropIndex("dbo.BenUsers", new[] { "UserProfileInfo_UserProfileInfoId" });
            DropIndex("dbo.BenUsers", "UserNameIndex");
            DropIndex("dbo.BenUsers", new[] { "CardUser" });
            DropIndex("dbo.UserRoles", new[] { "RoleId" });
            DropIndex("dbo.UserRoles", new[] { "UserId" });
            DropIndex("dbo.Roles", "RoleNameIndex");
            DropIndex("dbo.ReportFiles", new[] { "ReportTypeId" });
            DropIndex("dbo.OfferPages", new[] { "OfferTypeId" });
            DropIndex("dbo.ImageSections", new[] { "ImageSectionPageName" });
            DropIndex("dbo.ImageSections", new[] { "ImageSectionPageId" });
            DropIndex("dbo.DocumentFiles", new[] { "DocumentTypeId" });
            DropIndex("dbo.PromocionPages", new[] { "FosePageId" });
            DropIndex("dbo.ProductPages", new[] { "ProductId" });
            DropIndex("dbo.ProductPages", new[] { "PromocionPageId" });
            DropIndex("dbo.Products", new[] { "BlogTypeId" });
            DropIndex("dbo.Products", new[] { "OfferTypeId" });
            DropIndex("dbo.Products", new[] { "ServiceTypeId" });
            DropIndex("dbo.Products", new[] { "OfferSectionId" });
            DropIndex("dbo.Products", new[] { "BlogSectionId" });
            DropIndex("dbo.Products", new[] { "ProductTitle_TitleTypeId" });
            DropIndex("dbo.Products", new[] { "ProductName" });
            DropIndex("dbo.HomePages", new[] { "BlogSectionId" });
            DropIndex("dbo.HomePages", new[] { "FoseSectionId" });
            DropIndex("dbo.HomePages", new[] { "CardSectionId" });
            DropIndex("dbo.HomePages", new[] { "OfferSectionId" });
            DropIndex("dbo.NewsPages", new[] { "BlogPageId" });
            DropIndex("dbo.BlogPages", new[] { "BlogTypeId" });
            DropIndex("dbo.BillingPages", new[] { "LabSectionId" });
            DropIndex("dbo.BillingPages", new[] { "IncrementBenefitSectionId" });
            DropIndex("dbo.BillingPages", new[] { "BenefitSectionId" });
            DropIndex("dbo.Branches", new[] { "State_Id" , "City_Id"});
            DropIndex("dbo.EstadosMunicipios", new[] { "Estado_id", "Municipio_Id" });
            DropTable("dbo.UserLogins");
            DropTable("dbo.UserClaims");
            DropTable("dbo.BenUsers");
            DropTable("dbo.UserProfileInfos");
            DropTable("dbo.UserJoinedToTeams");
            DropTable("dbo.ServicePages");
            DropTable("dbo.SaDTypeNumbers");
            DropTable("dbo.SaDPages");
            DropTable("dbo.UserRoles");
            DropTable("dbo.Roles");
            DropTable("dbo.ReportTypes");
            DropTable("dbo.ReportFiles");
            DropTable("dbo.ProviderPages");
            DropTable("dbo.PrivacityPages");
            DropTable("dbo.PagePreviews");
            DropTable("dbo.OfferPages");
            DropTable("dbo.JoinTeamPages");
            DropTable("dbo.InvestorPages");
            DropTable("dbo.InterestAreas");
            DropTable("dbo.ImageSections");
            DropTable("dbo.DocumentTypes");
            DropTable("dbo.DocumentFiles");
            DropTable("dbo.ContactUsers");
            DropTable("dbo.ContactPages");
            DropTable("dbo.ConditionsTermsPages");
            DropTable("dbo.Branches");
            DropTable("dbo.BranchPages");
            DropTable("dbo.ServiceTypes");
            DropTable("dbo.TitleTypes");
            DropTable("dbo.FosePages");
            DropTable("dbo.PromocionPages");
            DropTable("dbo.ProductPages");
            DropTable("dbo.OfferTypes");
            DropTable("dbo.Products");
            DropTable("dbo.OfferSections");
            DropTable("dbo.FoseSections");
            DropTable("dbo.CardSections");
            DropTable("dbo.HomePages");
            DropTable("dbo.BlogSections");
            DropTable("dbo.NewsPages");
            DropTable("dbo.BlogTypes");
            DropTable("dbo.BlogPages");
            DropTable("dbo.LabSections");
            DropTable("dbo.IncrementBenefitSections");
            DropTable("dbo.BillingPages");
            DropTable("dbo.BenefitSections");
            DropTable("dbo.EstadosMunicipios");
            DropTable("dbo.Municipios");
            DropTable("dbo.Estados");
        }
    }
}
