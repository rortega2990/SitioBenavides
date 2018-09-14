using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using BenavidesFarm.DataModels.Models.Pages;
using BenavidesFarm.DataModels.Models.Pages.Elements;
using BenavidesFarm.DataModels.Models.Pages.Sections;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BenavidesFarm.DataModels.Models
{
    /// <summary>
    /// Clase que representa el contexto de la Base de Datos
    /// Aquí se incluyen todos los conjuntos de los objetos de las clases que representan tablas en la base de datos
    /// </summary>
    public class MyApplicationDbContext : IdentityDbContext<MyApplicationUser, MyApplicationRole, int, MyApplicationUserLogin, MyApplicationUserRole, MyApplicationUserClaim>
    {
        public MyApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<MyApplicationRole>().ToTable("Roles", "dbo");
            modelBuilder.Entity<MyApplicationUser>().ToTable("BenUsers", "dbo");
            modelBuilder.Entity<MyApplicationUserLogin>().ToTable("UserLogins", "dbo");
            modelBuilder.Entity<MyApplicationUserClaim>().ToTable("UserClaims", "dbo");
            modelBuilder.Entity<MyApplicationUserRole>().ToTable("UserRoles", "dbo");
            modelBuilder.Entity<UserProfileInfo>().ToTable("UserProfileInfos", "dbo");
           
        }

        public static MyApplicationDbContext Create()
        {
            return new MyApplicationDbContext();
        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        /* Cada vez que se incluya un nuevo modelo para la base de datos, se debe registrarlo acá como un DbSet
           para que se cree en la BD una vez que se inicialice el sitio web.*/
        
        public DbSet<PagePreview> PagePreviews { get; set; }

        public DbSet<ConditionsTermsPage> ConditionsTermsPages { get; set; }

        public DbSet<InterestArea> InterestAreas { get; set; }

        public DbSet<DocumentType> DocumentTypes { get; set; }

        public DbSet<OfferType> OfferTypes { get; set; }

        public DbSet<BlogType> BlogTypes { get; set; }

        public DbSet<DocumentFiles> DocumentFiles { get; set; }

        public DbSet<ContactPage> ContactPages { get; set; }

        public DbSet<BlogPage> BlogPages { get; set; }

        public DbSet<NewsPage> NewsPages { get; set; }

        public DbSet<ProviderPage> ProviderPages { get; set; }

        public DbSet<SaDPage> SaDPages { get; set; }

        public DbSet<PrivacityPage> PrivacityPages { get; set; }

        public DbSet<SaDTypeNumber> SaDTypeNumbers { get; set; }

        public DbSet<ServiceType> ServiceTypes { get; set; }

        public DbSet<UserProfileInfo> UserProfileInfos { get; set; }

        public DbSet<UserJoinedToTeam> UserJoinedToTeams { get; set; }

        public DbSet<ContactUser> ContactUsers { get; set; }

        public DbSet<ServicePage> ServicePages { get; set; }

        public DbSet<FosePage> FosePages { get; set; }

        public DbSet<PromocionPage> PromocionPages { get; set; }


        public DbSet<OfferPage> OfferPages { get; set; }

        public DbSet<JoinTeamPage> JoinTeamPages { get; set; }

        public DbSet<InvestorPage> InvestorPages { get; set; }

        public DbSet<BenefitSection> BenefitSections { get; set; }

        public DbSet<BillingPage> BillingPages { get; set; }

        public DbSet<BlogSection> BlogSections { get; set; }

        public DbSet<Branch> Branchs { get; set; }

        public DbSet<ReportType> ReportTypes { get; set; }

        public DbSet<ReportFiles> ReportFiles { get; set; }

        public DbSet<BranchPage> BranchPages { get; set; }

        public DbSet<CardSection> CardSections { get; set; }

        public DbSet<HomePage> HomePages { get; set; }

        public DbSet<ImageSection> ImageSections { get; set; }

        public DbSet<IncrementBenefitSection> IncrementBenefitSections { get; set; }

        public DbSet<LabSection> LabSections { get; set; }

        public DbSet<FoseSection> FoseSections { get; set; }

        public DbSet<OfferSection> OfferSections { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductPage> ProductPages { get; set; }

        public DbSet<TitleType> TitleTypes { get; set; }

        public DbSet<Estados> Estados { get; set; }

        public DbSet<Municipios> Municipios { get; set; }

        public DbSet<EstadosMunicipios> EstadosMunicipios { get; set; }

        public DbSet<Laboratory> Laboratories { get; set; }

        public DbSet<FourQuadSection> FourQuadSections { get; set; }

        public DbSet<DoctorsOfficeSection> DoctorsOfficeSections { get; set; }

        public DbSet<WhoWeArePage> WhoWeArePages { get; set; }

        public DbSet<WhoWeAreSimpleRowItemWithImage> WhoWeAreSimpleRowItemWithImageSections { get; set; }

        public DbSet<WhoWeAreTitledSection> WhoWeAreTitledSections { get; set; }

        public DbSet<DoctorsOfficePage> DoctorsOfficePages { get; set; }

        public DbSet<InterestRegion> InterestRegions { get; set; }

        public DbSet<MailConfiguration> MailConfigurations { get; set; }

        public DbSet<Pillar> Pillars { get; set; }

        public DbSet<Quote> Quotes { get; set; }

        public DbSet<PillarPage> PillarPages { get; set; }


    }
}