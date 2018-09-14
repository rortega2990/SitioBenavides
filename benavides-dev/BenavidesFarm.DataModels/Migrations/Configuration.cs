using BenavidesFarm.DataModels.Models.Pages.Elements;

namespace BenavidesFarm.DataModels.Migrations
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using Models.Pages;
    using Models.Pages.Sections;
    using Microsoft.AspNet.Identity;
    using Models;

    /// <summary>
    /// Clase para la Configuración de la BD
    /// Inicializa los datos cada vez que se inicia el sitio web.
    /// Cuando hay cambios en el modelo de datos se debe correr el comando
    /// add-migration Initial en la consola de Package Manager, una vez que se cambie alguna clase del
    /// modelo de datos se debe ejecutar ese comando y se generará otra clase parecida a esta 201704230945223_Initial con otra 
    /// numeración en el nombre del archivo con la fecha en que se ejecutó.
    /// Si se cambia el modelo de datos, es dedcir se agrega o se elimina un campo o un nuevo modelo
    /// se debe ejecutar ese comando para que se vaya creando el histórico de los cambios de la BD
    /// </summary>
    public sealed class Configuration : DbMigrationsConfiguration<MyApplicationDbContext>
    {
        /// <summary>
        /// Constructor de la clase de Configuración
        /// </summary>
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        /// <summary>
        /// Inicializa la BD con un usuario y el rol de Admin
        /// </summary>
        /// <param name="db"></param>
        public static void InitializeIdentityForEF(MyApplicationDbContext db)
        {
            const string card = "1234-5678-9012-3456";
            const string userName = "jud.macias@gmail.com";
            const string password = "Ben@vides*.2017";
            const string roleName = "Admin";

            var roleManager = new MyApplicationRoleManager(new MyApplicationRoleStore(db));
            var userManager = new MyApplicationUserManager(new MyApplicationUserStore(db));
            IdentityResult resultRole = null;
            
            //Crear el rol de Admin si éste no existe en la BD
            var role = roleManager.FindByName(roleName);
            if (role == null)
            {
                role = new MyApplicationRole(roleName, "Role Admin");
                resultRole = roleManager.Create(role);
            }

            //Crear el usuario si éste no existe en la BD
            var uCard = userManager.FindByCard(card);
            var uName = userManager.FindByName(userName);
            if (uCard == null && uName == null)
            {
                DateTime? date = new DateTime(2014, 10, 23);
                var user = new MyApplicationUser
                {
                    UserName = userName,
                    Email = userName,
                    CardUser = card,
                    PhoneNumber = "(111) 111-1111",
                    TypeUser = "WebAdmin",
                    UserProfileInfo = new UserProfileInfo
                    {
                        UserNames = "Judith Elizabeth",
                        UserLastName1 = "Macias",
                        UserLastName2 = "Palomeque",
                        UserBirthDate = new DateTime(1990, 10, 15),
                        UserFemale = true,
                        UserHasChildren = false,
                        UserClubPeques = true,
                        UserMount = 50.0f,
                        UserCreationDate = date.Value,
                        UserUpdateDate = date.Value,
                        UserCreationDateClubPeques = date,
                        UserImagePerfil = "~/Content/rsc/imgs/user_lg.png",
                        UserCity = "Monterrey, Nuevo Leon",
                        UserCodePostal = "67189",
                    }
                };
                IdentityResult resultUser = userManager.Create(user, password);

                if (resultUser == IdentityResult.Success && resultRole == IdentityResult.Success)
                {
                    userManager.SetLockoutEnabled(user.Id, false);

                    // Adicionar el rol de Admin al usuario si éste no lo tiene aún
                    var rolesForUser = userManager.GetRoles(user.Id);
                    if (!rolesForUser.Contains(role.Name))
                    {
                        userManager.AddToRole(user.Id, role.Name);
                    }
                }
            }
        }

        /// <summary>
        /// Método que se ejecuta en la inicialización de datos de la BD
        /// (Hay que comprobar que no existan los datos para que no los vuelva a crear)
        /// </summary>
        /// <param name="context"></param>
        protected override void Seed(MyApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            initializeStatesAndCities(context);

            InitializeIdentityForEF(context);

            saveBranchs(context);

            saveInterestAreas(context);

            saveReportTypes(context);

            saveSaDTypeNumbers(context);

            saveDocumentTypes(context);

            saveServiceTypes(context);

            var offerTypes = saveOfferTypes(context);

            var blogTypes = saveBlogTypes(context);

            var offerSections = saveOfferSections(context);

            var cardSections = saveCardSections(context);

            var blogSections = saveBlogSections(context);

            var benefitSections = saveBenefitSections(context);

            var incrementBenefitSections = saveIncrementBenefitSections(context);

            var foseSections = saveFosePages(context);

            saveOfferPages(context, offerTypes);

            saveBillingPages(context, benefitSections, incrementBenefitSections);

            saveBranchPages(context);

            var fourQuadSection = saveFourQuadSection(context);

            var doctorsOfficeSection = saveDoctorsOfficeSection(context);

            saveHomePages(context, offerSections, cardSections, foseSections, blogSections, fourQuadSection, doctorsOfficeSection);

            saveJoinTeamPages(context);

            saveInvestorPages(context);

            saveServicePages(context);

            saveSaDPages(context);

            saveContactPages(context);

            saveProviderPages(context);

            savePrivacityPages(context);

            saveConditionsTermsPages(context);

            saveBlogPages(context, blogTypes);

            saveWhoWeArePage(context);

            saveDoctorsOfficePage(context);

            saveInterestRegions(context);

            savePillars(context);

            saveQuotes(context);

            savePillarPages(context);
        }

        private static void saveDoctorsOfficePage(MyApplicationDbContext context)
        {
            var currentPage = context.DoctorsOfficePages.FirstOrDefault();
            if(currentPage == null)
            {
                var page = new DoctorsOfficePage() {
                    Active = true,
                    CreationDate = DateTime.Now,
                    HeadImages = new List<DoctorsOfficePageSection>()
                    {
                        new DoctorsOfficePageSection(){
                            ImageFileName = "~/Content/rsc/imgs/doctors-office-banner.png",
                            Title = "",
                            TitleColor = ""
                        }
                    },
                    ServicesSection = new List<DoctorsOfficePageSection>()
                    {
                         new DoctorsOfficePageSection(){
                            ImageFileName = "~/Content/rsc/imgs/doctors-office-services-01.png",
                            Title = "consulta médica",
                            TitleColor = "#323232"
                         },
                         new DoctorsOfficePageSection(){
                            ImageFileName = "~/Content/rsc/imgs/doctors-office-services-02.png",
                            Title = "inyecciones",
                            TitleColor = "#323232"
                         },
                         new DoctorsOfficePageSection(){
                            ImageFileName = "~/Content/rsc/imgs/doctors-office-services-03.png",
                            Title = "certificado médico",
                            TitleColor = "#323232"
                         },
                         new DoctorsOfficePageSection(){
                            ImageFileName = "~/Content/rsc/imgs/doctors-office-services-04.png",
                            Title = "toma de presión arterial",
                            TitleColor = "#323232"
                         },
                         new DoctorsOfficePageSection(){
                            ImageFileName = "~/Content/rsc/imgs/doctors-office-services-05.png",
                            Title = "curaciones",
                            TitleColor = "#323232"
                         }

                    }
                };

                context.DoctorsOfficePages.Add(page);
                context.SaveChanges();
            }
        }

        private static void saveWhoWeArePage(MyApplicationDbContext context)
        {
            var currentPage = context.WhoWeArePages.FirstOrDefault();
            if (currentPage == null)
            {

                var page = new WhoWeArePage()
                {
                    WhoWeAreSection = new WhoWeAreSimpleRowItem()
                    {
                        Title = "¿Quiénes Somos?",
                        TitleTextColor = "#abe1fa",
                        Message = "Líder en el mercado de venta a detalle de productos de salud y bienestar. Con 100 años de experiencia, tenemos presencia en más de 183 ciudades en 26 estados. Contamos con una red de 1,100 farmacias, 500 consultorios y un centro de distribución de alta tecnología. La cadena operada por casi 9,200 personas, anualmente atiende a más de 100 millones de clientes ofreciendo un catálogo de más de 16,000 diferentes productos de marcas comerciales y propias enfocados a ayudar a la salud y bienestar de sus clientes.",
                        MessageTextColor = "#fff",
                        BackgroundColor = " #27327b"
                    },
                    VisionSection = new WhoWeAreSimpleRowItem()
                    {
                        Title = "Nuestra visión",
                        TitleTextColor = "#27327b",
                        Message = "Ser la cadena de farmacias líder en México que cuenta con la mejor oferta de soluciones de salud y bienestar, reconocidos por nuestro servicio, rapidez, conveniencia, innovación y confiabilidad.",
                        MessageTextColor = "##27327b",
                        BackgroundColor = "#ecebec"
                    },
                    MisionSection = new WhoWeAreSimpleRowItem()
                    {
                        Title = "Nuestra misión",
                        TitleTextColor = "#27327b",
                        Message = "Ayudamos a las personas a mejorar su calidad de vida a través de soluciones integrales de salud y bienestar.",
                        MessageTextColor = "#27327b",
                        BackgroundColor = "#abe1fa"
                    },
                    AdSection = new WhoWeAreSimpleRowItemWithImage()
                    {
                        Message = "Ser la cadena de farmacias líder en México que cuenta con la mejor oferta de soluciones de salud y bienestar, reconocidos por nuestro servicio, rapidez, conveniencia, innovación y confiabilidad.",
                        MessageTextColor = "#20317c",
                        ImageFileName = "~/Content/rsc/imgs/who-we-are-ad.png",
                        BackgroundColor = "#fff"
                    },
                    ValuesSection = new List<WhoWeAreTitledSection>()
                {
                    new WhoWeAreTitledSection() {
                        Title = "Colaboración",
                        TileColor = "#fff",
                        Text = "Trabajamos de forma colaborativa entre nosotros y con nuestros socios para que todos salgamos ganando.",
                        TextColor = "#fff",
                        ImageFileName = "~/Content/rsc/imgs/values-collaboration.png"

                    },
                    new WhoWeAreTitledSection() {
                        Title = "Confianza",
                        TileColor = "#fff",
                        Text = "El respeto, la integridad y la franqueza nos guían para actuar correctamente.",
                        TextColor = "#fff",
                        ImageFileName = "~/Content/rsc/imgs/values-trust.png"
                    },
                    new WhoWeAreTitledSection() {
                        Title = "Innovación",
                        TileColor = "#fff",
                        Text = "Cultivamos una mente abierta y un espíritu empresarial en todo lo que hacemos.",
                        TextColor = "#fff",
                        ImageFileName = "~/Content/rsc/imgs/values-innovation.png"
                    },
                    new WhoWeAreTitledSection() {
                        Title = "Cuidado",
                        TileColor = "#fff",
                        Text = "Nuestro personal y nuestros clientes nos inspiran a actuar con compromiso y pasión.",
                        TextColor = "#fff",
                        ImageFileName = "~/Content/rsc/imgs/values-care.png"
                    },
                    new WhoWeAreTitledSection() {
                        Title = "Dedicación",
                        TileColor = "#fff",
                        Text = "Trabajamos con rigor, simplicidad y agilidad para brindar resultados excepcionales.",
                        TextColor = "#fff",
                        ImageFileName = "~/Content/rsc/imgs/values-dedication.png"
                    }
                },
                    HistoryImages = new List<WhoWeAreTitledSection>()
                {
                    new WhoWeAreTitledSection(){
                        Title="1917",
                        TileColor = "#fff",
                        SubTitle = "Los inicios",
                        SubTitleColor = "#fff",
                        Text = "Ésta década inició la expansión en el mercado fronterizo.",
                        TextColor = "#fff",
                        ImageFileName = "~/Content/rsc/imgs/history1.png"
                    },
                    new WhoWeAreTitledSection(){
                        Title="1943",
                        TileColor = "#fff",
                        SubTitle = "Crecimiento",
                        SubTitleColor = "#fff",
                        Text = "Década de los 60, con el firme propósito de consolidarse en Monterrey, se adquirieron las Bóticas Moebius y las Farmacias San Rafael. También en esta época inició el fortalecimiento del área del Pacífico, con la adquisición de 20 sucursales de la cadena Farmacias Bótica Moderna, S.A.",
                        TextColor = "#fff",
                        ImageFileName = "~/Content/rsc/imgs/history2.png"
                    },
                    new WhoWeAreTitledSection(){
                        Title="1950",
                        TileColor = "#fff",
                        SubTitle = "Mercado Fronterizo",
                        SubTitleColor = "#fff",
                        Text = "Contando con 168 sucursales, Benavides adquirió dos cadenas de Farmaciasen la zona Centro y Occidente del país, finalizando en el año de 1989 con 206 sucursales.",
                        TextColor = "#fff",
                        ImageFileName = "~/Content/rsc/imgs/history3.png"
                    },
                    new WhoWeAreTitledSection(){
                        Title="1960",
                        TileColor = "#fff",
                        SubTitle = "Consolidación",
                        SubTitleColor = "#fff",
                        Text = "Las farmacias adquiridas en este años fueron las Tikos y Las Palmas, ubicadas en Torreón y Durango, así como las farmacias Levy de Guadalajara.",
                        TextColor = "#fff",
                        ImageFileName = "~/Content/rsc/imgs/history4.png"
                    }
                },
                    HeadImages = new List<WhoWeAreTitledSection>()
                {
                    new WhoWeAreTitledSection()
                    {
                        ImageFileName = "~/Content/rsc/imgs/who-we-are-banner.png"
                    }
                },
                    Active = true,
                    CreationDate = DateTime.Now

                };

                context.WhoWeArePages.Add(page);
                context.SaveChanges();
            }
        }

        private static void initializeStatesAndCities(MyApplicationDbContext context)
        {
            var state = context.Estados.FirstOrDefault();
            var city = context.Municipios.FirstOrDefault();

            if (state == null && city == null)
            {
                context.Database.ExecuteSqlCommand(SqlFileContainer.States);
                context.Database.ExecuteSqlCommand(SqlFileContainer.Cities);
                context.Database.ExecuteSqlCommand(SqlFileContainer.States_Cities);
            }
        }

        private static void saveBlogPages(MyApplicationDbContext context, List<BlogType> blogTypes)
        {
            if (!context.BlogPages.Any())
            {
                var blogPages = new List<BlogPage>
                {
                    new BlogPage
                    {
                        BlogPageCustomValue = "BlogPage_Belleza_1",
                        BlogPageTitle = "Blog",
                        BlogPageActive = "Activada",
                        BlogPageCreatedDate = DateTime.Now,
                        BlogPageImage = "~/Content/rsc/imgs/gusto.png",
                        BlogTypeId = blogTypes[0].BlogTypeId,
                        BlogPageColorBgHead = "#48c7f3",
                        BlogPageColorTextHead = "#ffffff",
                        BlogPageTextHead = "Mi Belleza",
                        BlogPageColorBgTitleDesc = "#2a3479",
                        BlogPageColorTitleDesc = "#ffffff",
                        BlogPageTitleDesc = "Snacks que te mantendrán el estrés fuera",
                        BlogPageColorTextDescHead = "#ffffff",
                        BlogPageTextDesc =
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                    },
                    new BlogPage
                    {
                        BlogPageCustomValue = "BlogPage_Salud_1",
                        BlogPageTitle = "Blog",
                        BlogPageActive = "Activada",
                        BlogPageCreatedDate = DateTime.Now,
                        BlogPageImage = "~/Content/rsc/imgs/gusto.png",
                        BlogTypeId = blogTypes[1].BlogTypeId,
                        BlogPageColorBgHead = "#48c7f3",
                        BlogPageColorTextHead = "#ffffff",
                        BlogPageTextHead = "Mi Salud",
                        BlogPageColorBgTitleDesc = "#2a3479",
                        BlogPageColorTitleDesc = "#ffffff",
                        BlogPageTitleDesc = "Snacks que te mantendrán el estrés fuera",
                        BlogPageColorTextDescHead = "#ffffff",
                        BlogPageTextDesc =
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                    },
                    new BlogPage
                    {
                        BlogPageCustomValue = "BlogPage_Bebé_1",
                        BlogPageTitle = "Blog",
                        BlogPageActive = "Activada",
                        BlogPageCreatedDate = DateTime.Now,
                        BlogPageImage = "~/Content/rsc/imgs/gusto.png",
                        BlogTypeId = blogTypes[2].BlogTypeId,
                        BlogPageColorBgHead = "#48c7f3",
                        BlogPageColorTextHead = "#ffffff",
                        BlogPageTextHead = "Mi Bebé",
                        BlogPageColorBgTitleDesc = "#2a3479",
                        BlogPageColorTitleDesc = "#ffffff",
                        BlogPageTitleDesc = "Snacks que te mantendrán el estrés fuera",
                        BlogPageColorTextDescHead = "#ffffff",
                        BlogPageTextDesc =
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                    },
                    new BlogPage
                    {
                        BlogPageCustomValue = "BlogPage_Gustos_1",
                        BlogPageTitle = "Blog",
                        BlogPageActive = "Activada",
                        BlogPageCreatedDate = DateTime.Now,
                        BlogPageImage = "~/Content/rsc/imgs/gusto.png",
                        BlogTypeId = blogTypes[3].BlogTypeId,
                        BlogPageColorBgHead = "#48c7f3",
                        BlogPageColorTextHead = "#ffffff",
                        BlogPageTextHead = "Mis Gustos",
                        BlogPageColorBgTitleDesc = "#2a3479",
                        BlogPageColorTitleDesc = "#ffffff",
                        BlogPageTitleDesc = "Snacks que te mantendrán el estrés fuera",
                        BlogPageColorTextDescHead = "#ffffff",
                        BlogPageTextDesc =
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                    },
                };

                blogPages.ForEach(a => context.BlogPages.AddOrUpdate(h => h.BlogPageCustomValue, a));
                context.SaveChanges();

                var newsPages = new List<NewsPage>
                {
                    new NewsPage
                    {
                        NewsPageCustomValue = "NewsPage_1",
                        NewsPageTitle = "Noticias",
                        NewsPageActive = "Activada",
                        NewsPageCreatedDate = DateTime.Now,
                        NewsPageImageHead = "~/Content/rsc/imgs/gusto.png",
                        BlogPageId = 1,
                        NewsPageColorBgHead = "#48c7f3",
                        NewsPageColorBgSubTextHead = "#2a3479",
                        NewsPageColorSubTextHead = "#ffffff",
                        NewsPageColorTextHead = "#ffffff",
                        NewsPageSubTextHead = "¿Cómo combatir las alergias estacionales?",
                        NewsPageTextHead = "Mi Belleza",
                        NewsPageImageDescription = "~/Content/rsc/imgs/gusto.png",
                        NewsPageTitleDescription1 = "Lorem ipsum dolor sit amet  consectetur adipisicing elit, sed d",
                        NewsPageColorTitleDescription1 = "#48c7f3",
                        NewsPageTitleDescription2 = "Lorem ipsum dolor sit amet  consectetur adipisicing elit, sed d",
                        NewsPageColorTitleDescription2 = "#48c7f3",
                        NewsPageTextDescription1 =
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                        NewsPageTextDescription2 =
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                        NewsPageOrder = 1
                    },
                    new NewsPage
                    {
                        NewsPageCustomValue = "NewsPage_2",
                        NewsPageTitle = "Noticias",
                        NewsPageActive = "Activada",
                        NewsPageCreatedDate = DateTime.Now,
                        NewsPageImageHead = "~/Content/rsc/imgs/gusto.png",
                        BlogPageId = 1,
                        NewsPageColorBgHead = "#48c7f3",
                        NewsPageColorBgSubTextHead = "#2a3479",
                        NewsPageColorSubTextHead = "#ffffff",
                        NewsPageColorTextHead = "#ffffff",
                        NewsPageSubTextHead = "¿Cómo combatir las alergias estacionales?",
                        NewsPageTextHead = "Mi Belleza",
                        NewsPageImageDescription = "~/Content/rsc/imgs/gusto.png",
                        NewsPageTitleDescription1 = "Lorem ipsum dolor sit amet  consectetur adipisicing elit, sed d",
                        NewsPageColorTitleDescription1 = "#48c7f3",
                        NewsPageTitleDescription2 = "Lorem ipsum dolor sit amet  consectetur adipisicing elit, sed d",
                        NewsPageColorTitleDescription2 = "#48c7f3",
                        NewsPageTextDescription1 =
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                        NewsPageTextDescription2 =
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                        NewsPageOrder = 2,
                    },
                    new NewsPage
                    {
                        NewsPageCustomValue = "NewsPage_3",
                        NewsPageTitle = "Noticias",
                        NewsPageActive = "Activada",
                        NewsPageCreatedDate = DateTime.Now,
                        NewsPageImageHead = "~/Content/rsc/imgs/gusto.png",
                        BlogPageId = 2,
                        NewsPageColorBgHead = "#48c7f3",
                        NewsPageColorBgSubTextHead = "#2a3479",
                        NewsPageColorSubTextHead = "#ffffff",
                        NewsPageColorTextHead = "#ffffff",
                        NewsPageSubTextHead = "¿Cómo combatir las alergias estacionales?",
                        NewsPageTextHead = "Mi Belleza",
                        NewsPageImageDescription = "~/Content/rsc/imgs/gusto.png",
                        NewsPageTitleDescription1 = "Lorem ipsum dolor sit amet  consectetur adipisicing elit, sed d",
                        NewsPageColorTitleDescription1 = "#48c7f3",
                        NewsPageTitleDescription2 = "Lorem ipsum dolor sit amet  consectetur adipisicing elit, sed d",
                        NewsPageColorTitleDescription2 = "#48c7f3",
                        NewsPageTextDescription1 =
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                        NewsPageTextDescription2 =
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                        NewsPageOrder = 1
                    },
                    new NewsPage
                    {
                        NewsPageCustomValue = "NewsPage_4",
                        NewsPageTitle = "Noticias",
                        NewsPageActive = "Activada",
                        NewsPageCreatedDate = DateTime.Now,
                        NewsPageImageHead = "~/Content/rsc/imgs/gusto.png",
                        BlogPageId = 2,
                        NewsPageColorBgHead = "#48c7f3",
                        NewsPageColorBgSubTextHead = "#2a3479",
                        NewsPageColorSubTextHead = "#ffffff",
                        NewsPageColorTextHead = "#ffffff",
                        NewsPageSubTextHead = "¿Cómo combatir las alergias estacionales?",
                        NewsPageTextHead = "Mi Belleza",
                        NewsPageImageDescription = "~/Content/rsc/imgs/gusto.png",
                        NewsPageTitleDescription1 = "Lorem ipsum dolor sit amet  consectetur adipisicing elit, sed d",
                        NewsPageColorTitleDescription1 = "#48c7f3",
                        NewsPageTitleDescription2 = "Lorem ipsum dolor sit amet  consectetur adipisicing elit, sed d",
                        NewsPageColorTitleDescription2 = "#48c7f3",
                        NewsPageTextDescription1 =
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                        NewsPageTextDescription2 =
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                        NewsPageOrder = 2,
                    },
                    new NewsPage
                    {
                        NewsPageCustomValue = "NewsPage_5",
                        NewsPageTitle = "Noticias",
                        NewsPageActive = "Activada",
                        NewsPageCreatedDate = DateTime.Now,
                        NewsPageImageHead = "~/Content/rsc/imgs/gusto.png",
                        BlogPageId = 3,
                        NewsPageColorBgHead = "#48c7f3",
                        NewsPageColorBgSubTextHead = "#2a3479",
                        NewsPageColorSubTextHead = "#ffffff",
                        NewsPageColorTextHead = "#ffffff",
                        NewsPageSubTextHead = "¿Cómo combatir las alergias estacionales?",
                        NewsPageTextHead = "Mi Belleza",
                        NewsPageImageDescription = "~/Content/rsc/imgs/gusto.png",
                        NewsPageTitleDescription1 = "Lorem ipsum dolor sit amet  consectetur adipisicing elit, sed d",
                        NewsPageColorTitleDescription1 = "#48c7f3",
                        NewsPageTitleDescription2 = "Lorem ipsum dolor sit amet  consectetur adipisicing elit, sed d",
                        NewsPageColorTitleDescription2 = "#48c7f3",
                        NewsPageTextDescription1 =
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                        NewsPageTextDescription2 =
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                        NewsPageOrder = 1
                    },
                    new NewsPage
                    {
                        NewsPageCustomValue = "NewsPage_6",
                        NewsPageTitle = "Noticias",
                        NewsPageActive = "Activada",
                        NewsPageCreatedDate = DateTime.Now,
                        NewsPageImageHead = "~/Content/rsc/imgs/gusto.png",
                        BlogPageId = 3,
                        NewsPageColorBgHead = "#48c7f3",
                        NewsPageColorBgSubTextHead = "#2a3479",
                        NewsPageColorSubTextHead = "#ffffff",
                        NewsPageColorTextHead = "#ffffff",
                        NewsPageSubTextHead = "¿Cómo combatir las alergias estacionales?",
                        NewsPageTextHead = "Mi Belleza",
                        NewsPageImageDescription = "~/Content/rsc/imgs/gusto.png",
                        NewsPageTitleDescription1 = "Lorem ipsum dolor sit amet  consectetur adipisicing elit, sed d",
                        NewsPageColorTitleDescription1 = "#48c7f3",
                        NewsPageTitleDescription2 = "Lorem ipsum dolor sit amet  consectetur adipisicing elit, sed d",
                        NewsPageColorTitleDescription2 = "#48c7f3",
                        NewsPageTextDescription1 =
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                        NewsPageTextDescription2 =
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                        NewsPageOrder = 2,
                    },
                    new NewsPage
                    {
                        NewsPageCustomValue = "NewsPage_7",
                        NewsPageTitle = "Noticias",
                        NewsPageActive = "Activada",
                        NewsPageCreatedDate = DateTime.Now,
                        NewsPageImageHead = "~/Content/rsc/imgs/gusto.png",
                        BlogPageId = 4,
                        NewsPageColorBgHead = "#48c7f3",
                        NewsPageColorBgSubTextHead = "#2a3479",
                        NewsPageColorSubTextHead = "#ffffff",
                        NewsPageColorTextHead = "#ffffff",
                        NewsPageSubTextHead = "¿Cómo combatir las alergias estacionales?",
                        NewsPageTextHead = "Mi Belleza",
                        NewsPageImageDescription = "~/Content/rsc/imgs/gusto.png",
                        NewsPageTitleDescription1 = "Lorem ipsum dolor sit amet  consectetur adipisicing elit, sed d",
                        NewsPageColorTitleDescription1 = "#48c7f3",
                        NewsPageTitleDescription2 = "Lorem ipsum dolor sit amet  consectetur adipisicing elit, sed d",
                        NewsPageColorTitleDescription2 = "#48c7f3",
                        NewsPageTextDescription1 =
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                        NewsPageTextDescription2 =
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                        NewsPageOrder = 1
                    },
                    new NewsPage
                    {
                        NewsPageCustomValue = "NewsPage_8",
                        NewsPageTitle = "Noticias",
                        NewsPageActive = "Activada",
                        NewsPageCreatedDate = DateTime.Now,
                        NewsPageImageHead = "~/Content/rsc/imgs/gusto.png",
                        BlogPageId = 4,
                        NewsPageColorBgHead = "#48c7f3",
                        NewsPageColorBgSubTextHead = "#2a3479",
                        NewsPageColorSubTextHead = "#ffffff",
                        NewsPageColorTextHead = "#ffffff",
                        NewsPageSubTextHead = "¿Cómo combatir las alergias estacionales?",
                        NewsPageTextHead = "Mi Belleza",
                        NewsPageImageDescription = "~/Content/rsc/imgs/gusto.png",
                        NewsPageTitleDescription1 = "Lorem ipsum dolor sit amet  consectetur adipisicing elit, sed d",
                        NewsPageColorTitleDescription1 = "#48c7f3",
                        NewsPageTitleDescription2 = "Lorem ipsum dolor sit amet  consectetur adipisicing elit, sed d",
                        NewsPageColorTitleDescription2 = "#48c7f3",
                        NewsPageTextDescription1 =
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                        NewsPageTextDescription2 =
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                        NewsPageOrder = 2,
                    },
                };

                newsPages.ForEach(a => context.NewsPages.AddOrUpdate(h => h.NewsPageCustomValue, a));
                context.SaveChanges();
            }
        }

        private static void saveConditionsTermsPages(MyApplicationDbContext context)
        {
            if (!context.ConditionsTermsPages.Any())
            {
                var terms = new List<ConditionsTermsPage>
                {
                    new ConditionsTermsPage
                    {
                        ConditionsTermsPageActive = true,
                        ConditionsTermsPageBgColorHead = "#48c7f3",
                        ConditionsTermsPageColorHeadText = "#ffffff",
                        ConditionsTermsPageHeadText = "Términos y Condiciones",
                        ConditionsTermsPageCreatedDate = DateTime.Now,
                        ConditionsTermsPageCustomValue = "ConditionsTermsPage_1",
                        ConditionsTermsPageTitle = "Términos y Condiciones",
                        ConditionsTermsPageTextTitle =
                            "En cumplimiento a lo dispuesto en la Ley Federal de Protección de Datos Personales en Posesión de los Particulares, FARMACIAS BENAVIDES, S.A.B. DE C.V. (en lo sucesivo “BENAVIDES”), con domicilio en Avenida Fundadores # 935 int. 301, Colonia Valle del Mirador, Código Postal 64750, en el Municipio de Monterrey, Nuevo Leon, es responsable de recabar sus datos personales, del uso que se le dé a los mismos y de su protección.",
                        ConditionsTermsPageTextColor = "#213997",
                        ConditionsTermsPageTextDescription =
                            @"La recopilación de ciertos datos personales inclusive, los considerados sensibles de clientes, proveedores, empleados y candidatos a empleados son recabados con la finalidad que su misma naturaleza permite y que es imprescindible para llevar a cabo y ofrecerle operaciones activas, pasivas y de servicio que Usted celebre con nosotros, su información personal será utilizada para proveerle los servicios y productos que ha solicitado o que promocione “BENAVIDES”. Dichos datos personales pueden haber sido o pueden ser obtenidos de Usted, ya sea personalmente o bien, directamente por cualquier medio electrónico, óptico, sonoro, visual, o a través de cualquier otra tecnología. Asímismo, podemos obtener datos personales de los que usted es titular, a través de terceros y de otras fuentes permitidas por la Ley.

        Para las finalidades antes mencionadas, requerimos obtener los siguientes datos personales, de manera enunciativa más no limitativa: Datos de identificación, nombre completo, dirección, teléfono de casa, celular y/o de trabajo, estado civil, firma, correo electrónico, R.F.C., C.U.R.P., lugar y fecha de nacimiento, edad, nombres de familiares, dependientes y beneficiarios, así como sus domicilios, entre otros; Datos laborales: ocupación, puesto, área o departamento, domicilio, teléfono y correo de trabajo, referencias laborales, referencias personales y referencias comerciales, entre otros; Datos personales sensibles: afiliación sindical, creencias religiosas y otros que pueden ser así considerados.

        Todos los Datos Personales proporcionados a “BENAVIDES”, serán considerados como información confidencial, obligación que subsistirá a pesar de que el Usuario haya finalizado su relación con “BENAVIDES”, asimismo dichos Datos Personales no serán divulgados a terceras personas sin la autorización del Usuario.

        A este respecto, el Usuario en este acto autoriza a “BENAVIDES” a transferir los Datos Personales proporcionados, a cualquiera de sus afiliadas, subsidiarias, partes relacionadas o socios comerciales, ya sean nacionales o extranjeros, los cuales, en su caso, destinarán los Datos Personales a la misma finalidad descrita en el presente Aviso de Privacidad. De conformidad con lo dispuesto en la Ley Federal de Protección de Datos Personales en Posesión de Particulares, a partir del 6 de Enero de 2012, el titular por sí o mediante representante legal debidamente acreditado, podrá ejercer sus derechos de acceso, rectificación, cancelación y oposición para cancelar sus datos personales, así como de oponerse al tratamiento de los mismos o revocar el consentimiento que para tal fin nos haya otorgado, presentando su solicitud a través de escrito dirigido al Representante Legal de Farmacias Benavides, S.A.B. de C.V., a la dirección de Avenida Fundadores # 935 int. 301, Colonia Valle del Mirador, Código Postal 64750, en el Municipio de Monterrey, Nuevo Leon o por correo electrónico a datos_privacidad@benavides.com.mx Nos reservamos el derecho de efectuar en cualquier momento cambios o modificaciones al presente aviso de privacidad, cualquier cambio o modificación a este aviso de privacidad se hará de su conocimiento en el sitio de Internet www.benavides.com.mx Fecha de la última actualización 29 de agosto del 2011,"
                    }
                };

                terms.ForEach(a => context.ConditionsTermsPages.AddOrUpdate(h => h.ConditionsTermsPageCustomValue, a));
                context.SaveChanges();
            }
        }

        private static void savePrivacityPages(MyApplicationDbContext context)
        {
            if (!context.PrivacityPages.Any())
            {
                var privacity = new List<PrivacityPage>
                {
                    new PrivacityPage
                    {
                        PrivacityPageActive = true,
                        PrivacityPageBgColorHead = "#48c7f3",
                        PrivacityPageColorHeadText = "#ffffff",
                        PrivacityPageHeadText = "Privacidad",
                        PrivacityPageCreatedDate = DateTime.Now,
                        PrivacityPageCustomValue = "PrivacityPage_1",
                        PrivacityPageTitle = "Privacidad",
                        PrivacityPageTextTitle =
                            "En cumplimiento a lo dispuesto en la Ley Federal de Protección de Datos Personales en Posesión de los Particulares, FARMACIAS BENAVIDES, S.A.B. DE C.V. (en lo sucesivo “BENAVIDES”), con domicilio en Avenida Fundadores # 935 int. 301, Colonia Valle del Mirador, Código Postal 64750, en el Municipio de Monterrey, Nuevo Leon, es responsable de recabar sus datos personales, del uso que se le dé a los mismos y de su protección.",
                        PrivacityPageTextColor = "#213997",
                        PrivacityPageTextDescription =
                            @"La recopilación de ciertos datos personales inclusive, los considerados sensibles de clientes, proveedores, empleados y candidatos a empleados son recabados con la finalidad que su misma naturaleza permite y que es imprescindible para llevar a cabo y ofrecerle operaciones activas, pasivas y de servicio que Usted celebre con nosotros, su información personal será utilizada para proveerle los servicios y productos que ha solicitado o que promocione “BENAVIDES”. Dichos datos personales pueden haber sido o pueden ser obtenidos de Usted, ya sea personalmente o bien, directamente por cualquier medio electrónico, óptico, sonoro, visual, o a través de cualquier otra tecnología. Asímismo, podemos obtener datos personales de los que usted es titular, a través de terceros y de otras fuentes permitidas por la Ley.

        Para las finalidades antes mencionadas, requerimos obtener los siguientes datos personales, de manera enunciativa más no limitativa: Datos de identificación, nombre completo, dirección, teléfono de casa, celular y/o de trabajo, estado civil, firma, correo electrónico, R.F.C., C.U.R.P., lugar y fecha de nacimiento, edad, nombres de familiares, dependientes y beneficiarios, así como sus domicilios, entre otros; Datos laborales: ocupación, puesto, área o departamento, domicilio, teléfono y correo de trabajo, referencias laborales, referencias personales y referencias comerciales, entre otros; Datos personales sensibles: afiliación sindical, creencias religiosas y otros que pueden ser así considerados.

        Todos los Datos Personales proporcionados a “BENAVIDES”, serán considerados como información confidencial, obligación que subsistirá a pesar de que el Usuario haya finalizado su relación con “BENAVIDES”, asimismo dichos Datos Personales no serán divulgados a terceras personas sin la autorización del Usuario.

        A este respecto, el Usuario en este acto autoriza a “BENAVIDES” a transferir los Datos Personales proporcionados, a cualquiera de sus afiliadas, subsidiarias, partes relacionadas o socios comerciales, ya sean nacionales o extranjeros, los cuales, en su caso, destinarán los Datos Personales a la misma finalidad descrita en el presente Aviso de Privacidad. De conformidad con lo dispuesto en la Ley Federal de Protección de Datos Personales en Posesión de Particulares, a partir del 6 de Enero de 2012, el titular por sí o mediante representante legal debidamente acreditado, podrá ejercer sus derechos de acceso, rectificación, cancelación y oposición para cancelar sus datos personales, así como de oponerse al tratamiento de los mismos o revocar el consentimiento que para tal fin nos haya otorgado, presentando su solicitud a través de escrito dirigido al Representante Legal de Farmacias Benavides, S.A.B. de C.V., a la dirección de Avenida Fundadores # 935 int. 301, Colonia Valle del Mirador, Código Postal 64750, en el Municipio de Monterrey, Nuevo Leon o por correo electrónico a datos_privacidad@benavides.com.mx Nos reservamos el derecho de efectuar en cualquier momento cambios o modificaciones al presente aviso de privacidad, cualquier cambio o modificación a este aviso de privacidad se hará de su conocimiento en el sitio de Internet www.benavides.com.mx Fecha de la última actualización 29 de agosto del 2011,"
                    }
                };

                privacity.ForEach(a => context.PrivacityPages.AddOrUpdate(h => h.PrivacityPageCustomValue, a));
                context.SaveChanges();
            }
        }

        private static void saveDocumentTypes(MyApplicationDbContext context)
        {
            if (!context.DocumentTypes.Any())
            {
                var DocumentTypes = new List<DocumentType>
                {
                    new DocumentType
                    {
                        DocumentActive = true,
                        DocumentDescription = "Formatos requeridos de diversos trámites y procesos en formato Excel.",
                        DocumentName = "Formatos",
                        ProviderFiles = new List<DocumentFiles>
                        {
                            new DocumentFiles
                            {
                                AddressFile = "~/ProviderDocuments/doc1.excel",
                                NameDescriptiveFile = "Formato de incorporación de proveedores"
                            },
                            new DocumentFiles
                            {
                                AddressFile = "~/ProviderDocuments/doc2.excel",
                                NameDescriptiveFile = "Formato de ficha técnica"
                            },
                            new DocumentFiles
                            {
                                AddressFile = "~/ProviderDocuments/doc3.excel",
                                NameDescriptiveFile = "Formato de alta de artículos"
                            }
                        }
                    },
                    new DocumentType
                    {
                        DocumentActive = true,
                        DocumentDescription = "Manuales para proveedores en formato PDF.",
                        DocumentName = "Documentos y manuales",
                        ProviderFiles = new List<DocumentFiles>
                        {
                            new DocumentFiles
                            {
                                AddressFile = "~/ProviderDocuments/doc1.pdf",
                                NameDescriptiveFile = "Manual para proveedores"
                            },
                            new DocumentFiles
                            {
                                AddressFile = "~/ProviderDocuments/doc2.pdf",
                                NameDescriptiveFile = "Condiciones de importación"
                            }
                        }
                    }
                };

                DocumentTypes.ForEach(a => context.DocumentTypes.AddOrUpdate(h => h.DocumentName, a));
                context.SaveChanges();
            }
        }

        private static void saveProviderPages(MyApplicationDbContext context)
        {
            if (!context.ProviderPages.Any())
            {
                var providers = new List<ProviderPage>
                {
                    new ProviderPage
                    {
                        ProviderPageActive = true,
                        ProviderPageBgColorHead = "#48c7f3",
                        ProviderPageColorHeadText = "#ffffff",
                        ProviderPageHeadText = "Proveedores",
                        ProviderPageCreatedDate = DateTime.Now,
                        ProviderPageCustomValue = "ProviderPage_1",
                        ProviderPageTitle = "Proveedores",
                        ProviderPageSubText =
                            "En esta sección podrán encontrar formas, manuales y documentos de utilidad para todos los proveedores de Farmacias Benavides.",
                        ProviderPageColorSubText = "#213997",
                    }
                };

                providers.ForEach(a => context.ProviderPages.AddOrUpdate(h => h.ProviderPageCustomValue, a));
                context.SaveChanges();
            }
        }

        private static void saveContactPages(MyApplicationDbContext context)
        {
            if (!context.ContactPages.Any())
            {
                var contacts = new List<ContactPage>
                {
                    new ContactPage
                    {
                        ContactPageActive = true,
                        ContactPageBgColorHead = "#48c7f3",
                        ContactPageColorHeadText = "#ffffff",
                        ContactPageHeadText = "Contacto",
                        ContactPageCreatedDate = DateTime.Now,
                        ContactPageCustomValue = "ContactPage_1",
                        ContactPageTitle = "Contacto",
                        ContactPageColorTextFooter = "#2a3479",
                        ContactPageAddress =
                            "Avenida Fundadores No. 935 interior 301 Col. Valle del Mirador C.P. 64750 Monterrey, N.L.",
                        ContactPageTelAddress = "81 8150 77 00",
                        ContactPageTelSaD = "01800 835 2800",
                        ContactPageTelAaP = "81 8150 77 00",
                        ContactPageEmailSaD = "cliente@benavides.com.mx",
                        ContactPageSubText1 = "El personal de Farmacias Benavides se encuentra disponible en todo momento para ayudarlo a resolver cualquier duda.",
                        ContactPageSubText2 =
                            "Por favor llene los campos requeridos (*) y nos comunicaremos con usted a la brevedad posible. O vea nuestro directorio de contactos para ver los teléfonos de nuestro personal.",
                        ContactPageColorSubText1 = "#2a3479",
                        ContactPageColorSubText2 = "#2a3479"
                    }
                };

                contacts.ForEach(a => context.ContactPages.AddOrUpdate(h => h.ContactPageCustomValue, a));
                context.SaveChanges();
            }
        }

        private static void saveSaDTypeNumbers(MyApplicationDbContext context)
        {
            if (!context.SaDTypeNumbers.Any())
            {
                var saDTypes = new List<SaDTypeNumber>
                {
                    new SaDTypeNumber
                    {
                        SaDTypeNumberActive = true,
                        SaDTypeNumberCity = "Aguascalientes, Ags.",
                        SaDTypeNumberPhone = "250-1000",
                    },
                    new SaDTypeNumber
                    {
                        SaDTypeNumberActive = true,
                        SaDTypeNumberCity = "Chihuahua, Chih.",
                        SaDTypeNumberPhone = "440-3040"
                    },
                    new SaDTypeNumber
                    {
                        SaDTypeNumberActive = true,
                        SaDTypeNumberCity = "León, Gto.",
                        SaDTypeNumberPhone = "148-6600"
                    },
                    new SaDTypeNumber
                    {
                        SaDTypeNumberActive = true,
                        SaDTypeNumberCity = "Monclova, Coah.",
                        SaDTypeNumberPhone = "635-8100"
                    },
                    new SaDTypeNumber
                    {
                        SaDTypeNumberActive = true,
                        SaDTypeNumberCity = "Sabinas Hidalgo, N.L.",
                        SaDTypeNumberPhone = "148-6600"
                    },
                    new SaDTypeNumber
                    {
                        SaDTypeNumberActive = true,
                        SaDTypeNumberCity = "Tijuana, B.C.",
                        SaDTypeNumberPhone = "685-1111"
                    },
                    new SaDTypeNumber
                    {
                        SaDTypeNumberActive = true,
                        SaDTypeNumberCity = "Cadereyta, N.L.",
                        SaDTypeNumberPhone = "284-0404"
                    },
                    new SaDTypeNumber
                    {
                        SaDTypeNumberActive = true,
                        SaDTypeNumberCity = "Culiacán, Sin.",
                        SaDTypeNumberPhone = "716-1600"
                    },
                    new SaDTypeNumber
                    {
                        SaDTypeNumberActive = true,
                        SaDTypeNumberCity = "Linares, N.L.",
                        SaDTypeNumberPhone = "212-1212"
                    },
                    new SaDTypeNumber
                    {
                        SaDTypeNumberActive = true,
                        SaDTypeNumberCity = "Monterrey, N.L.",
                        SaDTypeNumberPhone = "8126-0000"
                    },
                    new SaDTypeNumber
                    {
                        SaDTypeNumberActive = true,
                        SaDTypeNumberCity = "Saltillo, Coah.",
                        SaDTypeNumberPhone = "722-02222"
                    },
                    new SaDTypeNumber
                    {
                        SaDTypeNumberActive = true,
                        SaDTypeNumberCity = "Cd. Juárez, Chih.",
                        SaDTypeNumberPhone = "648-0808"
                    },
                    new SaDTypeNumber
                    {
                        SaDTypeNumberActive = true,
                        SaDTypeNumberCity = "Durango, Dgo.",
                        SaDTypeNumberPhone = "812-0101"
                    },
                    new SaDTypeNumber
                    {
                        SaDTypeNumberActive = true,
                        SaDTypeNumberCity = "Matamoros, Tamps.",
                        SaDTypeNumberPhone = "817-0717"
                    },
                    new SaDTypeNumber
                    {
                        SaDTypeNumberActive = true,
                        SaDTypeNumberCity = "Morelia, Mich.",
                        SaDTypeNumberPhone = "312-9700"
                    },
                    new SaDTypeNumber
                    {
                        SaDTypeNumberActive = true,
                        SaDTypeNumberCity = "San Mateo Atenco, Edo. de México",
                        SaDTypeNumberPhone = "287-2100"
                    },
                    new SaDTypeNumber
                    {
                        SaDTypeNumberActive = true,
                        SaDTypeNumberCity = "Puerto Vallarta, Jal.",
                        SaDTypeNumberPhone = "187-0777"
                    },
                    new SaDTypeNumber
                    {
                        SaDTypeNumberActive = true,
                        SaDTypeNumberCity = "Cd. Obregón, Son.",
                        SaDTypeNumberPhone = "414-5050"
                    },
                    new SaDTypeNumber
                    {
                        SaDTypeNumberActive = true,
                        SaDTypeNumberCity = "Guadalajara, Jal.",
                        SaDTypeNumberPhone = "3834-4000"
                    },
                    new SaDTypeNumber
                    {
                        SaDTypeNumberActive = true,
                        SaDTypeNumberCity = "Metepec Toluca, Edo. de México",
                        SaDTypeNumberPhone = "218-1100"
                    },
                    new SaDTypeNumber
                    {
                        SaDTypeNumberActive = true,
                        SaDTypeNumberCity = "Querétaro, Qro.",
                        SaDTypeNumberPhone = "234-2777"
                    },
                    new SaDTypeNumber
                    {
                        SaDTypeNumberActive = true,
                        SaDTypeNumberCity = "San Buenaventura, Coah.",
                        SaDTypeNumberPhone = "694-2000"
                    },
                    new SaDTypeNumber
                    {
                        SaDTypeNumberActive = true,
                        SaDTypeNumberCity = "Zamora, Mich.",
                        SaDTypeNumberPhone = "512-2100"
                    },
                    new SaDTypeNumber
                    {
                        SaDTypeNumberActive = true,
                        SaDTypeNumberCity = "Cd. Victoria, Tamps.",
                        SaDTypeNumberPhone = "312-1444"
                    },
                    new SaDTypeNumber
                    {
                        SaDTypeNumberActive = true,
                        SaDTypeNumberCity = "Hermosillo, Son.",
                        SaDTypeNumberPhone = "212-5555"
                    },
                    new SaDTypeNumber
                    {
                        SaDTypeNumberActive = true,
                        SaDTypeNumberCity = "Mexicali, B.C.",
                        SaDTypeNumberPhone = "552-5353"
                    },
                    new SaDTypeNumber
                    {
                        SaDTypeNumberActive = true,
                        SaDTypeNumberCity = "Reynosa, Tamps.",
                        SaDTypeNumberPhone = "926-2600"
                    },
                    new SaDTypeNumber
                    {
                        SaDTypeNumberActive = true,
                        SaDTypeNumberCity = "Tampico, Tamps.",
                        SaDTypeNumberPhone = "227-0027"
                    },
                    new SaDTypeNumber
                    {
                        SaDTypeNumberActive = true,
                        SaDTypeNumberCity = "Zapotlanejo, Jal.",
                        SaDTypeNumberPhone = "735-3535"
                    },
                    new SaDTypeNumber
                    {
                        SaDTypeNumberActive = true,
                        SaDTypeNumberCity = "Chapala, Jal.",
                        SaDTypeNumberPhone = "765-6525"
                    },
                    new SaDTypeNumber
                    {
                        SaDTypeNumberActive = true,
                        SaDTypeNumberCity = "Laredo, Tamps.",
                        SaDTypeNumberPhone = "714-7000"
                    },
                    new SaDTypeNumber
                    {
                        SaDTypeNumberActive = true,
                        SaDTypeNumberCity = "Ciudad de México",
                        SaDTypeNumberPhone = "5868-4000"
                    },
                    new SaDTypeNumber
                    {
                        SaDTypeNumberActive = true,
                        SaDTypeNumberCity = "Río Bravo, Tamps.",
                        SaDTypeNumberPhone = "926-2600"
                    },
                    new SaDTypeNumber
                    {
                        SaDTypeNumberActive = true,
                        SaDTypeNumberCity = "Tepic, Nay.",
                        SaDTypeNumberPhone = "213-3131"
                    },
                };
                saDTypes.ForEach(a => context.SaDTypeNumbers.AddOrUpdate(h => h.SaDTypeNumberCity, a));
                context.SaveChanges();
            }
        }

        private static void saveSaDPages(MyApplicationDbContext context)
        {
            if (!context.SaDPages.Any())
            {
                var saDPages = new List<SaDPage>
                {
                    new SaDPage
                    {
                        SaDPageCustomValue = "SaDPage_1",
                        SaDPageTitle = "Servicio a Domicilio",
                        SaDPageActive = true,
                        SaDPageCreatedDate = DateTime.Now,
                        SaDPageHeadText1 = "no salgas,",
                        SaDPageHeadText2 = "nosotros te lo llevamos",
                        SaDPageHeadTextColor1 = "#ffffff",
                        SaDPageHeadTextColor2 = "#ffffff",
                        SaDPageImageBg = "~/Content/rsc/imgs//fondoSaD.png",
                        SaDPageImageLogo = "~/Content/rsc/imgs/bannerSaD.png",
                        SaDPageSubText1 = "Servicio a Domicilio",
                        SaDPageSubTextColor1 = "#2a3479",
                        SaDPageNumberPrincipalText = "01 800 248 5555",
                        SaDPageNumberPrincipalBgColor = "#ff0000",
                        SaDPageNumberPrincipalTextColor = "#ffffff"
                    }
                };
                saDPages.ForEach(a => context.SaDPages.AddOrUpdate(h => h.SaDPageCustomValue, a));
                context.SaveChanges();
            }
        }

        private static void saveServicePages(MyApplicationDbContext context)
        {
            if (!context.ServicePages.Any())
            {
                var servicePages = new List<ServicePage>
                {
                    new ServicePage
                    {
                        ServicePageCustomValue = "ServicePage_1",
                        ServicePageTitle = "Servicios",
                        ServicePageActive = true,
                        ServicePageCreatedDate = DateTime.Now,
                        ServicePageHeadText1 = "Paga tus Servicios",
                        ServicePageHeadSubText1 = "sin ir tan lejos y sin filas",
                        ServicePageColorHeadText1 = "#2a3479",
                        ServicePageColorHeadSubText1 = "#ffffff",
                        ServicePageColorHeadBg = "#48c7f3",
                        ServicePageColorSubText = "#ec2028",
                        ServicePageColorSubTextDescription = "",
                        ServicePageSubText = "Todas tus vueltas en una vuelta",
                        ServicePageSubTextDescription =
                            "Surte tu receta médica, paga tus servicios, deposita dinero, recarga tiempo aire y mucho más. Visita tu farmacia más cercana. Y además, si no puedes venir, nosotros te lo llevamos.",
                        ServicePageImageLogo = "~/Content/rsc/imgs/recibito.png"
                    }
                };
                servicePages.ForEach(a => context.ServicePages.AddOrUpdate(h => h.ServicePageCustomValue, a));
                context.SaveChanges();
            }
        }

        private static void saveServiceTypes(MyApplicationDbContext context)
        {
            if (!context.ServiceTypes.Any())
            {
                var serviceTypes = new List<ServiceType>
                {
                    new ServiceType
                    {
                        ServiceTypeName = "Paga tus servicios",
                        ServiceTypeNameDescription = "*aceptamos pagos vencidos",
                        ServiceTypeActive = true,
                        ServiceTypeProdutcsDescription = "",
                        Products = new List<Product>
                        {
                            new Product
                            {
                                ProductName = "Product_10",
                                ProductCustomValue = "Product_ServiceType_1",
                                ProductImage = "~/Content/rsc/imgs/pago1.png",
                                ProductOrder = 1,
                                ProductURL = ""
                            },
                            new Product
                            {
                                ProductName = "Product_11",
                                ProductCustomValue = "Product_ServiceType_1",
                                ProductImage = "~/Content/rsc/imgs/pago2.png",
                                ProductOrder = 2,
                                ProductURL = ""
                            },
                            new Product
                            {
                                ProductName = "Product_12",
                                ProductCustomValue = "Product_ServiceType_1",
                                ProductImage = "~/Content/rsc/imgs/pago3.png",
                                ProductOrder = 3,
                                ProductURL = ""
                            },
                            new Product
                            {
                                ProductName = "Product_13",
                                ProductCustomValue = "Product_ServiceType_1",
                                ProductImage = "~/Content/rsc/imgs/pago5.png",
                                ProductOrder = 4,
                                ProductURL = ""
                            },
                            new Product
                            {
                                ProductName = "Product_14",
                                ProductCustomValue = "Product_ServiceType_1",
                                ProductImage = "~/Content/rsc/imgs/pago6.png",
                                ProductOrder = 5,
                                ProductURL = ""
                            },
                            new Product
                            {
                                ProductName = "Product_15",
                                ProductCustomValue = "Product_ServiceType_1",
                                ProductImage = "~/Content/rsc/imgs/pago7.png",
                                ProductOrder = 6,
                                ProductURL = ""
                            },
                            new Product
                            {
                                ProductName = "Product_16",
                                ProductCustomValue = "Product_ServiceType_1",
                                ProductImage = "~/Content/rsc/imgs/pago8.png",
                                ProductOrder = 7,
                                ProductURL = ""
                            },
                            new Product
                            {
                                ProductName = "Product_17",
                                ProductCustomValue = "Product_ServiceType_1",
                                ProductImage = "~/Content/rsc/imgs/pago10.png",
                                ProductOrder = 8,
                                ProductURL = ""
                            },
                            new Product
                            {
                                ProductName = "Product_18",
                                ProductCustomValue = "Product_ServiceType_1",
                                ProductImage = "~/Content/rsc/imgs/pago11.png",
                                ProductOrder = 9,
                                ProductURL = ""
                            },
                            new Product
                            {
                                ProductName = "Product_19",
                                ProductCustomValue = "Product_ServiceType_1",
                                ProductImage = "~/Content/rsc/imgs/pago12.png",
                                ProductOrder = 10,
                                ProductURL = ""
                            },
                            new Product
                            {
                                ProductName = "Product_20",
                                ProductCustomValue = "Product_ServiceType_1",
                                ProductImage = "~/Content/rsc/imgs/pago13.png",
                                ProductOrder = 11,
                                ProductURL = ""
                            },
                            new Product
                            {
                                ProductName = "Product_21",
                                ProductCustomValue = "Product_ServiceType_1",
                                ProductImage = "~/Content/rsc/imgs/pago14.png",
                                ProductOrder = 12,
                                ProductURL = ""
                            },
                            new Product
                            {
                                ProductName = "Product_22",
                                ProductCustomValue = "Product_ServiceType_1",
                                ProductImage = "~/Content/rsc/imgs/pago15.png",
                                ProductOrder = 12,
                                ProductURL = ""
                            }
                        }
                    },
                    new ServiceType
                    {
                        ServiceTypeName = "Recarga tu celular",
                        ServiceTypeNameDescription = "",
                        ServiceTypeActive = true,
                        ServiceTypeProdutcsDescription = "",
                        Products = new List<Product>
                        {
                            new Product
                            {
                                ProductName = "Product_23",
                                ProductCustomValue = "Product_ServiceType_2",
                                ProductImage = "~/Content/rsc/imgs/cell1.png",
                                ProductOrder = 1,
                                ProductURL = ""
                            },
                            new Product
                            {
                                ProductName = "Product_24",
                                ProductCustomValue = "Product_ServiceType_2",
                                ProductImage = "~/Content/rsc/imgs/cell2.png",
                                ProductOrder = 2,
                                ProductURL = ""
                            },
                            new Product
                            {
                                ProductName = "Product_25",
                                ProductCustomValue = "Product_ServiceType_2",
                                ProductImage = "~/Content/rsc/imgs/cell3.png",
                                ProductOrder = 3,
                                ProductURL = ""
                            },
                            new Product
                            {
                                ProductName = "Product_26",
                                ProductCustomValue = "Product_ServiceType_2",
                                ProductImage = "~/Content/rsc/imgs/cell4.png",
                                ProductOrder = 4,
                                ProductURL = ""
                            },
                            new Product
                            {
                                ProductName = "Product_27",
                                ProductCustomValue = "Product_ServiceType_2",
                                ProductImage = "~/Content/rsc/imgs/cell5.png",
                                ProductOrder = 5,
                                ProductURL = ""
                            },
                        }
                    },
                    new ServiceType
                    {
                        ServiceTypeName = "Pago y depósito de tarjetas",
                        ServiceTypeNameDescription = "",
                        ServiceTypeActive = true,
                        ServiceTypeProdutcsDescription =
                            "Paga o deposita a tus tarjetas de crédito o débito de forma segura y al instante",
                        Products = new List<Product>
                        {
                            new Product
                            {
                                ProductName = "Product_28",
                                ProductCustomValue = "Product_ServiceType_3",
                                ProductImage = "~/Content/rsc/imgs/tarjeta1.png",
                                ProductOrder = 1,
                                ProductURL = ""
                            },
                            new Product
                            {
                                ProductName = "Product_29",
                                ProductCustomValue = "Product_ServiceType_3",
                                ProductImage = "~/Content/rsc/imgs/tarjeta2.png",
                                ProductOrder = 2,
                                ProductURL = ""
                            },
                        }
                    },
                    new ServiceType
                    {
                        ServiceTypeName = "Impresión de fotografías",
                        ServiceTypeNameDescription = "",
                        ServiceTypeActive = true,
                        ServiceTypeProdutcsDescription = "",
                        Products = new List<Product>
                        {
                            new Product
                            {
                                ProductName = "Product_30",
                                ProductCustomValue = "Product_ServiceType_4",
                                ProductImage = "~/Content/rsc/imgs/impresion1.png",
                                ProductOrder = 1,
                                ProductURL = ""
                            },
                            new Product
                            {
                                ProductName = "Product_31",
                                ProductCustomValue = "Product_ServiceType_4",
                                ProductImage = "~/Content/rsc/imgs/impresion2.png",
                                ProductOrder = 2,
                                ProductURL = ""
                            },
                            new Product
                            {
                                ProductName = "Product_32",
                                ProductCustomValue = "Product_ServiceType_4",
                                ProductImage = "~/Content/rsc/imgs/impresion3.png",
                                ProductOrder = 3,
                                ProductURL = ""
                            },
                        }
                    }
                };
                serviceTypes.ForEach(a => context.ServiceTypes.AddOrUpdate(h => h.ServiceTypeName, a));
                context.SaveChanges();
            }
        }

        private static void saveInvestorPages(MyApplicationDbContext context)
        {
            if (!context.InvestorPages.Any())
            {
                var investorPages = new List<InvestorPage>
                {
                    new InvestorPage
                    {
                        InvestorPageCustomValue = "InvestorPage_1",
                        InvestorPageTitle = "Inversionistas",
                        InvestorPageActive = true,
                        InvestorPageCreatedDate = DateTime.Now,
                        InvestorPageHeadText = "Inversionistas",
                        InvestorPageColorHeadText = "#ffffff",
                        InvestorPageColorHeadBg = "#48c7f3",
                        InvestorPageSubText = "Encuentra aquí todos nuestros reportes trimestrales para inversionistas",
                        InvestorPageColorSubText = "#48c7f3"
                    }
                };
                investorPages.ForEach(a => context.InvestorPages.AddOrUpdate(h => h.InvestorPageCustomValue, a));
                context.SaveChanges();
            }
        }

        private static void saveReportTypes(MyApplicationDbContext context)
        {
            if (!context.ReportTypes.Any())
            {
                var ReportTypes = new List<ReportType>
                {
                    new ReportType
                    {
                        ReportName = "Reportes Trimestrales",
                        ReportDescription = "Encuentra aquí todos nuestros reportes trimestrales para inversionistas.",
                        ReportActive = true
                    },
                    new ReportType
                    {
                        ReportName = "Reportes anuales",
                        ReportDescription = "Informes Anuales de Farmacias Benavides.",
                        ReportActive = true
                    },
                    new ReportType
                    {
                        ReportName = "Información BMV y CNBV",
                        ReportDescription =
                            "Informes trimestrales para la Bolsa Mexicana de Valores y la Comisión Nacional Bancaria de Valores.",
                        ReportActive = true
                    }
                };
                ReportTypes.ForEach(a => context.ReportTypes.AddOrUpdate(h => h.ReportName, a));
                context.SaveChanges();
            }
        }

        private static void saveInterestAreas(MyApplicationDbContext context)
        {
            if (!context.InterestAreas.Any())
            {
                var creationDate = DateTime.Now;

                var interestArea = new List<InterestArea>
                {
                    new InterestArea
                    {
                        InterestAreaName = "Mercadotecnia",
                        InterestAreaActive = true,
                        CreationDate = creationDate,
                        MailCollection = "babrego@benavides.com.mx; aguevara@benavides.com.mx; pdeosio@benavides.com.mx"
                    },
                    new InterestArea
                    {
                        InterestAreaName = "Administración",
                        InterestAreaActive = true,
                        CreationDate = creationDate,
                        MailCollection = "babrego@benavides.com.mx; aguevara@benavides.com.mx; pdeosio@benavides.com.mx"
                    },
                    new InterestArea
                    {
                        InterestAreaName = "Finanzas",
                        InterestAreaActive = true,
                        CreationDate = creationDate,
                        MailCollection = "babrego@benavides.com.mx; aguevara@benavides.com.mx; pdeosio@benavides.com.mx"
                    },
                    new InterestArea
                    {
                        InterestAreaName = "Legal",
                        InterestAreaActive = true,
                        CreationDate = creationDate,
                        MailCollection = "babrego@benavides.com.mx; aguevara@benavides.com.mx; pdeosio@benavides.com.mx"
                    },
                    new InterestArea
                    {
                        InterestAreaName = "Expansión",
                        InterestAreaActive = true,
                        CreationDate = creationDate,
                        MailCollection = "babrego@benavides.com.mx; aguevara@benavides.com.mx; pdeosio@benavides.com.mx"
                    },
                    new InterestArea
                    {
                        InterestAreaName = "TI",
                        InterestAreaActive = true,
                        CreationDate = creationDate,
                        MailCollection = "babrego@benavides.com.mx; aguevara@benavides.com.mx; pdeosio@benavides.com.mx"
                    },
                    new InterestArea
                    {
                        InterestAreaName = "Recursos Humanos",
                        InterestAreaActive = true,
                        CreationDate = creationDate,
                        MailCollection = "babrego@benavides.com.mx; aguevara@benavides.com.mx; pdeosio@benavides.com.mx"
                    },
                    new InterestArea
                    {
                        InterestAreaName = "Logística",
                        InterestAreaActive = true,
                        CreationDate = creationDate,
                        MailCollection = "babrego@benavides.com.mx; aguevara@benavides.com.mx; pdeosio@benavides.com.mx"
                    },
                    new InterestArea
                    {
                        InterestAreaName = "Call Center",
                        InterestAreaActive = true,
                        CreationDate = creationDate,
                        MailCollection = "tcampos@benavides.com.mx"
                    },
                    new InterestArea
                    {
                        InterestAreaName = "Compras",
                        InterestAreaActive = true,
                        CreationDate = creationDate,
                        MailCollection = "babrego@benavides.com.mx; aguevara@benavides.com.mx; pdeosio@benavides.com.mx"
                    },
                    new InterestArea
                    {
                        InterestAreaName = "Mostrador",
                        InterestAreaActive = true,
                        CreationDate = creationDate,
                        MailCollection = "nomail@noserver.com"
                    }
                };
                interestArea.ForEach(a => context.InterestAreas.AddOrUpdate(h => h.InterestAreaName, a));
                context.SaveChanges();
            }
        }

        private static void saveInterestRegions(MyApplicationDbContext context)
        {
            if(!context.InterestRegions.Any())
            {
                var creationDate = DateTime.Now;
               
                var regions = new List<InterestRegion>() {
                new InterestRegion()
                {
                    Name = "Monterrey",
                    Active = true,
                    CreationDate = creationDate,
                    MailCollection = "adlacruz@benavides.com.mx; amontejano@benavides.com.mx"
                },
                new InterestRegion()
                {
                    Name = "Jalisco",
                    Active = true,
                    CreationDate = creationDate,
                    MailCollection = "ogarcia@benavides.com.mx"
                },
                new InterestRegion()
                {
                    Name = "Mexicali",
                    Active = true,
                    CreationDate = creationDate,
                    MailCollection = "carana@benavides.com.mx; cmachado@benavides.com.mx"
                },
                new InterestRegion()
                {
                    Name = "Tijuana",
                    Active = true,
                    CreationDate = creationDate,
                    MailCollection = "carana@benavides.com.mx; cmachado@benavides.com.mx"
                },
                new InterestRegion()
                {
                    Name = "Sonora",
                    Active = true,
                    CreationDate = creationDate,
                    MailCollection = "carana@benavides.com.mx; cmachado@benavides.com.mx"
                },
                new InterestRegion()
                {
                    Name = "Tamaulipas",
                    Active = true,
                    CreationDate = creationDate,
                    MailCollection = "mmurillol@benavides.com.mx"
                },
                new InterestRegion()
                {
                    Name = "Veracruz",
                    Active = true,
                    CreationDate = creationDate,
                    MailCollection = "mmurillol@benavides.com.mx"
                },
                new InterestRegion()
                {
                    Name = "Coahuila",
                    Active = true,
                    CreationDate = creationDate,
                    MailCollection = "jjacquez@benavides.com.mx"
                },
                new InterestRegion()
                {
                    Name = "Chihuahua",
                    Active = true,
                    CreationDate = creationDate,
                    MailCollection = "jjacquez@benavides.com.mx"
                },
                new InterestRegion()
                {
                    Name = "León",
                    Active = true,
                    CreationDate = creationDate,
                    MailCollection = "chernandezr@benavides.com.mx"
                },
                new InterestRegion()
                {
                    Name = "Michoacán",
                    Active = true,
                    CreationDate = creationDate,
                    MailCollection = "chernandezr@benavides.com.mx"
                },
                new InterestRegion()
                {
                    Name = "CDMX",
                    Active = true,
                    CreationDate = creationDate,
                    MailCollection = "ematias@benavides.com.mx; apena@benavides.com.mx"
                },
                new InterestRegion()
                {
                    Name = "Estado de México",
                    Active = true,
                    CreationDate = creationDate,
                    MailCollection = "ematias@benavides.com.mx; apena@benavides.com.mx"
                },
                new InterestRegion()
                {
                    Name = "Querétaro",
                    Active = true,
                    CreationDate = creationDate,
                    MailCollection = "ematias@benavides.com.mx; apena@benavides.com.mx"
                },
                new InterestRegion()
                {
                    Name = "Puebla",
                    Active = true,
                    CreationDate = creationDate,
                    MailCollection = "ematias@benavides.com.mx; apena@benavides.com.mx"
                },
                new InterestRegion()
                {
                    Name = "Cuernavaca",
                    Active = true,
                    CreationDate = creationDate,
                    MailCollection = "ematias@benavides.com.mx; apena@benavides.com.mx"
                },
                new InterestRegion()
                {
                    Name = "Acapulco",
                    Active = true,
                    CreationDate = creationDate,
                    MailCollection = "ematias@benavides.com.mx; apena@benavides.com.mx"
                },
                };

                regions.ForEach(r => context.InterestRegions.AddOrUpdate(r));
                context.SaveChanges();
            }
        }

        private static void saveJoinTeamPages(MyApplicationDbContext context)
        {
            if (!context.JoinTeamPages.Any())
            {
                var joinTeamPages = new List<JoinTeamPage>
                {
                    new JoinTeamPage
                    {
                        JoinTeamPageCustomValue = "JoinTeamPage_1",
                        JoinTeamPageTitle = "Únete al equipo",
                        JoinTeamPageActive = true,
                        JoinTeamPageCreatedDate = DateTime.Now,
                        JoinTeamPageSubText1 = "Colabora con nuestro gran equipo de trabajo y haz crecer el bienestar de México",
                        JoinTeamPageSubText2 = "Encuentra nuestras vacantes, están publicadas en LinkedIn.",
                        JoinTeamPageColorText1 = "#2a3479",
                        JoinTeamPageColorText2 = "#2a3479",
                    }
                };
                joinTeamPages.ForEach(a => context.JoinTeamPages.AddOrUpdate(h => h.JoinTeamPageCustomValue, a));
                context.SaveChanges();

                var imageSections = new List<ImageSection>
                {
                    new ImageSection
                    {
                        ImageSectionImage = "~/Content/rsc/imgs/unete-banner.png",
                        ImageSectionPageId = joinTeamPages[0].JoinTeamPageId,
                        ImageSectionPageName = "JoinTeamPage",
                        ImageSectionText = "Únete al equipo",
                        ImageSectionColorText = "#ffffff",
                        ImageSectionActive = true
                    },
                };
                imageSections.ForEach(a => context.ImageSections.AddOrUpdate(a));
                context.SaveChanges();
            }
        }

        private static List<FoseSection> saveFosePages(MyApplicationDbContext context)
        {
            var FoseSections = new List<FoseSection>
            {
                new FoseSection
                {
                    FoseSectionCustomValue = "FoseSection_1",
                    FoseSectionTitle = "Boots Beauty Brands",
                    FoseSectionColorTitle = "#2a3479",
                    FoseSectionWord1 = "Descubre la nueva forma",
                    FoseSectionColorWord1 = "#b4b3b3",
                    FoseSectionColorWord2 = "#999292",
                    FoseSectionWord2 = "de comprar maquillaje",
                    FoseSectionImageLogo = "~/Content/rsc/imgs/makeup_logos.png",
                    FoseSectionImage = "~/Content/rsc/imgs/makeup.png"
                }
            };
            FoseSections.ForEach(a => context.FoseSections.AddOrUpdate(h => h.FoseSectionCustomValue, a));
            context.SaveChanges();

            if (!context.FosePages.Any())
            {
                var fosePages = new List<FosePage>
                {
                    new FosePage
                    {
                        FosePageCustomValue = "FosePage_1",
                        FosePageTitle = "Boots Beauty Brands",
                        FosePageActive = "Activada",
                        FosePageCreatedDate = DateTime.Now,
                        FoseTextBranch =
                            "Localiza <span class=\"color-red-dark text-43px\"><b>aquí</b></span> tu <span id=\"sucursal\" class=\"text-35px\">sucursal</span> más cercana <span class=\"text-35px\">con estos productos</span>",
                        Promocions = new List<PromocionPage>
                        {
                            new PromocionPage
                            {
                                PromocionPageColorHeadBg = "#ff9194",
                                PromocionPageImageLogo1 = "~/Content/rsc/imgs/logo-soap.png",
                                PromocionPageImageLogo2 = "~/Content/rsc/imgs/soap-logo.png",
                                PromocionPageHeadText = "Diviértete siendo tú misma",
                                PromocionPageSpanHeadText = "siendo tú misma",
                                PromocionPageHeadtextColor = "#ffffff",
                                PromocionPageSpanHeadtextColor = "#fed800",
                                PromocionPageSubText1 = "desde Inglaterra a tu farmacia",
                                PromocionPageSubText2 = "NUESTROS PRODUCTOS EXCLUSIVOS",
                                PromocionPageActive = "Activada",
                                PromocionPageCreatedDate = DateTime.Now,
                                PromocionPageCustomValue = "Promocion_1",
                                PromocionPageOrder = 1,
                                PromocionPageHeadImage = "~/Content/rsc/imgs/promo-1.jpg",
                                ProductPages = new List<ProductPage>
                                {
                                    new ProductPage
                                    {
                                        ProductPageActive = "Activada",
                                        ProductPageCreatedDate = DateTime.Now,
                                        ProductPageCustomValue = "ProductPage_1",
                                        Product = new Product
                                        {
                                            ProductName = "Product_33",
                                            ProductCustomValue = "Product_Promocion_1",
                                            ProductImage = "~/Content/rsc/imgs/soap-logo.png",
                                            ProductOrder = 1,
                                            ProductURL = "",
                                            ProductSubtitle = "Bright & Bubbly",
                                        },
                                        ProductPageBgColor = "#ff9194",
                                        ProductPageTextTitle = "Botanics",
                                        ProductPageColorTextTitle = "#2b7b6f",
                                        ProductPageTextDescription1 = "Botanics Cleansing toner all bright",
                                        ProductPageTextDescription2 =
                                            "Contrarresta la deshidratación de tu piel. Para todo tipo de piel",
                                        ProductPageTextCharacteristic1 = "limpia y purifica tu piel",
                                        ProductPageTextCharacteristic2 =
                                            "Contiene karité, aceite de macadamia y malvavisco.",
                                        ProductPageColorTextDescription1 = "#ffffff",
                                        ProductPageColorTextDescription2 = "#ffffff",
                                        ProductPageColorTextCharacteristic1 = "#ffffff",
                                        ProductPageColorTextCharacteristic2 = "#ffffff"
                                    },
                                    new ProductPage
                                    {
                                        ProductPageActive = "Activada",
                                        ProductPageCreatedDate = DateTime.Now,
                                        ProductPageCustomValue = "ProductPage_2",
                                        Product = new Product
                                        {
                                            ProductName = "Product_34",
                                            ProductCustomValue = "Product_Promocion_1",
                                            ProductImage = "~/Content/rsc/imgs/soap-logo.png",
                                            ProductOrder = 2,
                                            ProductURL = "",
                                            ProductSubtitle = "Winter Wonderhand"
                                        },
                                        ProductPageBgColor = "#ff9194",
                                        ProductPageTextTitle = "Botanics",
                                        ProductPageColorTextTitle = "#2b7b6f",
                                        ProductPageTextDescription1 = "Botanics Cleansing toner all bright",
                                        ProductPageTextDescription2 =
                                            "Contrarresta la deshidratación de tu piel. Para todo tipo de piel",
                                        ProductPageTextCharacteristic1 = "limpia y purifica tu piel",
                                        ProductPageTextCharacteristic2 =
                                            "Contiene karité, aceite de macadamia y malvavisco.",
                                        ProductPageColorTextDescription1 = "#ffffff",
                                        ProductPageColorTextDescription2 = "#ffffff",
                                        ProductPageColorTextCharacteristic1 = "#ffffff",
                                        ProductPageColorTextCharacteristic2 = "#ffffff"
                                    },
                                    new ProductPage
                                    {
                                        ProductPageActive = "Activada",
                                        ProductPageCreatedDate = DateTime.Now,
                                        ProductPageCustomValue = "ProductPage_3",
                                        Product = new Product
                                        {
                                            ProductName = "Product_35",
                                            ProductCustomValue = "Product_Promocion_1",
                                            ProductImage = "~/Content/rsc/imgs/soap-logo.png",
                                            ProductOrder = 3,
                                            ProductURL = "",
                                            ProductSubtitle = "Bright & Bubbly"
                                        },
                                        ProductPageBgColor = "#ff9194",
                                        ProductPageTextTitle = "Botanics",
                                        ProductPageColorTextTitle = "#2b7b6f",
                                        ProductPageTextDescription1 = "Botanics Cleansing toner all bright",
                                        ProductPageTextDescription2 =
                                            "Contrarresta la deshidratación de tu piel. Para todo tipo de piel",
                                        ProductPageTextCharacteristic1 = "limpia y purifica tu piel",
                                        ProductPageTextCharacteristic2 =
                                            "Contiene karité, aceite de macadamia y malvavisco.",
                                        ProductPageColorTextDescription1 = "#ffffff",
                                        ProductPageColorTextDescription2 = "#ffffff",
                                        ProductPageColorTextCharacteristic1 = "#ffffff",
                                        ProductPageColorTextCharacteristic2 = "#ffffff"
                                    },
                                    new ProductPage
                                    {
                                        ProductPageActive = "Activada",
                                        ProductPageCreatedDate = DateTime.Now,
                                        ProductPageCustomValue = "ProductPage_4",
                                        Product = new Product
                                        {
                                            ProductName = "Product_36",
                                            ProductCustomValue = "Product_Promocion_1",
                                            ProductImage = "~/Content/rsc/imgs/soap-logo.png",
                                            ProductOrder = 4,
                                            ProductURL = "",
                                            ProductSubtitle = "Winter Wonderhand"
                                        },
                                        ProductPageBgColor = "#ff9194",
                                        ProductPageTextTitle = "Botanics",
                                        ProductPageColorTextTitle = "#2b7b6f",
                                        ProductPageTextDescription1 = "Botanics Cleansing toner all bright",
                                        ProductPageTextDescription2 =
                                            "Contrarresta la deshidratación de tu piel. Para todo tipo de piel",
                                        ProductPageTextCharacteristic1 = "limpia y purifica tu piel",
                                        ProductPageTextCharacteristic2 =
                                            "Contiene karité, aceite de macadamia y malvavisco.",
                                        ProductPageColorTextDescription1 = "#ffffff",
                                        ProductPageColorTextDescription2 = "#ffffff",
                                        ProductPageColorTextCharacteristic1 = "#ffffff",
                                        ProductPageColorTextCharacteristic2 = "#ffffff"
                                    },
                                    new ProductPage
                                    {
                                        ProductPageActive = "Activada",
                                        ProductPageCreatedDate = DateTime.Now,
                                        ProductPageCustomValue = "ProductPage_5",
                                        Product = new Product
                                        {
                                            ProductName = "Product_37",
                                            ProductCustomValue = "Product_Promocion_1",
                                            ProductImage = "~/Content/rsc/imgs/soap-logo.png",
                                            ProductOrder = 5,
                                            ProductURL = "",
                                            ProductSubtitle = "Bright & Bubbly"
                                        },
                                        ProductPageBgColor = "#ff9194",
                                        ProductPageTextTitle = "Botanics",
                                        ProductPageColorTextTitle = "#2b7b6f",
                                        ProductPageTextDescription1 = "Botanics Cleansing toner all bright",
                                        ProductPageTextDescription2 =
                                            "Contrarresta la deshidratación de tu piel. Para todo tipo de piel",
                                        ProductPageTextCharacteristic1 = "limpia y purifica tu piel",
                                        ProductPageTextCharacteristic2 =
                                            "Contiene karité, aceite de macadamia y malvavisco.",
                                        ProductPageColorTextDescription1 = "#ffffff",
                                        ProductPageColorTextDescription2 = "#ffffff",
                                        ProductPageColorTextCharacteristic1 = "#ffffff",
                                        ProductPageColorTextCharacteristic2 = "#ffffff"
                                    },
                                    new ProductPage
                                    {
                                        ProductPageActive = "Activada",
                                        ProductPageCreatedDate = DateTime.Now,
                                        ProductPageCustomValue = "ProductPage_6",
                                        Product = new Product
                                        {
                                            ProductName = "Product_38",
                                            ProductCustomValue = "Product_Promocion_1",
                                            ProductImage = "~/Content/rsc/imgs/soap-logo.png",
                                            ProductOrder = 6,
                                            ProductURL = "",
                                            ProductSubtitle = "Winter Wonderhand"
                                        },
                                        ProductPageBgColor = "#ff9194",
                                        ProductPageTextTitle = "Botanics",
                                        ProductPageColorTextTitle = "#2b7b6f",
                                        ProductPageTextDescription1 = "Botanics Cleansing toner all bright",
                                        ProductPageTextDescription2 =
                                            "Contrarresta la deshidratación de tu piel. Para todo tipo de piel",
                                        ProductPageTextCharacteristic1 = "limpia y purifica tu piel",
                                        ProductPageTextCharacteristic2 =
                                            "Contiene karité, aceite de macadamia y malvavisco.",
                                        ProductPageColorTextDescription1 = "#ffffff",
                                        ProductPageColorTextDescription2 = "#ffffff",
                                        ProductPageColorTextCharacteristic1 = "#ffffff",
                                        ProductPageColorTextCharacteristic2 = "#ffffff"
                                    },
                                    new ProductPage
                                    {
                                        ProductPageActive = "Activada",
                                        ProductPageCreatedDate = DateTime.Now,
                                        ProductPageCustomValue = "ProductPage_7",
                                        Product = new Product
                                        {
                                            ProductName = "Product_39",
                                            ProductCustomValue = "Product_Promocion_1",
                                            ProductImage = "~/Content/rsc/imgs/soap-logo.png",
                                            ProductOrder = 7,
                                            ProductURL = "",
                                            ProductSubtitle = "Bright & Bubbly"
                                        },
                                        ProductPageBgColor = "#ff9194",
                                        ProductPageTextTitle = "Botanics",
                                        ProductPageColorTextTitle = "#2b7b6f",
                                        ProductPageTextDescription1 = "Botanics Cleansing toner all bright",
                                        ProductPageTextDescription2 =
                                            "Contrarresta la deshidratación de tu piel. Para todo tipo de piel",
                                        ProductPageTextCharacteristic1 = "limpia y purifica tu piel",
                                        ProductPageTextCharacteristic2 =
                                            "Contiene karité, aceite de macadamia y malvavisco.",
                                        ProductPageColorTextDescription1 = "#ffffff",
                                        ProductPageColorTextDescription2 = "#ffffff",
                                        ProductPageColorTextCharacteristic1 = "#ffffff",
                                        ProductPageColorTextCharacteristic2 = "#ffffff"
                                    },
                                    new ProductPage
                                    {
                                        ProductPageActive = "Activada",
                                        ProductPageCreatedDate = DateTime.Now,
                                        ProductPageCustomValue = "ProductPage_8",
                                        Product = new Product
                                        {
                                            ProductName = "Product_40",
                                            ProductCustomValue = "Product_Promocion_1",
                                            ProductImage = "~/Content/rsc/imgs/soap-logo.png",
                                            ProductOrder = 8,
                                            ProductURL = "",
                                            ProductSubtitle = "Winter Wonderhand"
                                        },
                                        ProductPageBgColor = "#ff9194",
                                        ProductPageTextTitle = "Botanics",
                                        ProductPageColorTextTitle = "#2b7b6f",
                                        ProductPageTextDescription1 = "Botanics Cleansing toner all bright",
                                        ProductPageTextDescription2 =
                                            "Contrarresta la deshidratación de tu piel. Para todo tipo de piel",
                                        ProductPageTextCharacteristic1 = "limpia y purifica tu piel",
                                        ProductPageTextCharacteristic2 =
                                            "Contiene karité, aceite de macadamia y malvavisco.",
                                        ProductPageColorTextDescription1 = "#ffffff",
                                        ProductPageColorTextDescription2 = "#ffffff",
                                        ProductPageColorTextCharacteristic1 = "#ffffff",
                                        ProductPageColorTextCharacteristic2 = "#ffffff"
                                    },
                                    new ProductPage
                                    {
                                        ProductPageActive = "Activada",
                                        ProductPageCreatedDate = DateTime.Now,
                                        ProductPageCustomValue = "ProductPage_9",
                                        Product = new Product
                                        {
                                            ProductName = "Product_41",
                                            ProductCustomValue = "Product_Promocion_1",
                                            ProductImage = "~/Content/rsc/imgs/soap-logo.png",
                                            ProductOrder = 9,
                                            ProductURL = "",
                                            ProductSubtitle = "Bright & Bubbly"
                                        },
                                        ProductPageBgColor = "#ff9194",
                                        ProductPageTextTitle = "Botanics",
                                        ProductPageColorTextTitle = "#2b7b6f",
                                        ProductPageTextDescription1 = "Botanics Cleansing toner all bright",
                                        ProductPageTextDescription2 =
                                            "Contrarresta la deshidratación de tu piel. Para todo tipo de piel",
                                        ProductPageTextCharacteristic1 = "limpia y purifica tu piel",
                                        ProductPageTextCharacteristic2 =
                                            "Contiene karité, aceite de macadamia y malvavisco.",
                                        ProductPageColorTextDescription1 = "#ffffff",
                                        ProductPageColorTextDescription2 = "#ffffff",
                                        ProductPageColorTextCharacteristic1 = "#ffffff",
                                        ProductPageColorTextCharacteristic2 = "#ffffff"
                                    },
                                }
                            },
                            new PromocionPage
                            {
                                PromocionPageColorHeadBg = "#000000",
                                PromocionPageImageLogo1 = "~/Content/rsc/imgs/foxe-logo.png",
                                PromocionPageImageLogo2 = "~/Content/rsc/imgs/foxe-producto.png",
                                PromocionPageHeadText = "Perfección a tu alcance",
                                PromocionPageSpanHeadText = "a tu alcance",
                                PromocionPageSpanHeadtextColor = "#fed800",
                                PromocionPageHeadtextColor = "#ffffff",
                                PromocionPageSubText1 = "desde Inglaterra a tu farmacia",
                                PromocionPageSubText2 = "NUESTROS PRODUCTOS EXCLUSIVOS",
                                PromocionPageActive = "Activada",
                                PromocionPageCreatedDate = DateTime.Now,
                                PromocionPageCustomValue = "Promocion_2",
                                PromocionPageOrder = 2,
                                PromocionPageHeadImage = "~/Content/rsc/imgs/promo-2.jpg",
                                ProductPages = new List<ProductPage>
                                {
                                    new ProductPage
                                    {
                                        ProductPageActive = "Activada",
                                        ProductPageCreatedDate = DateTime.Now,
                                        ProductPageCustomValue = "ProductPage_10",
                                        Product = new Product
                                        {
                                            ProductName = "Product_42",
                                            ProductCustomValue = "Product_Promocion_2",
                                            ProductImage = "~/Content/rsc/imgs/prueba.png",
                                            ProductOrder = 1,
                                            ProductURL = "",
                                            ProductSubtitle = "Cleasing Toner"
                                        },
                                        ProductPageBgColor = "#4bc8b6",
                                        ProductPageTextTitle = "Botanics",
                                        ProductPageColorTextTitle = "#2b7b6f",
                                        ProductPageTextDescription1 = "Botanics Cleansing toner all bright",
                                        ProductPageTextDescription2 =
                                            "Contrarresta la deshidratación de tu piel. Para todo tipo de piel",
                                        ProductPageTextCharacteristic1 = "limpia y purifica tu piel",
                                        ProductPageTextCharacteristic2 =
                                            "Contiene karité, aceite de macadamia y malvavisco.",
                                        ProductPageColorTextDescription1 = "#ffffff",
                                        ProductPageColorTextDescription2 = "#ffffff",
                                        ProductPageColorTextCharacteristic1 = "#ffffff",
                                        ProductPageColorTextCharacteristic2 = "#ffffff"
                                    },
                                    new ProductPage
                                    {
                                        ProductPageActive = "Activada",
                                        ProductPageCreatedDate = DateTime.Now,
                                        ProductPageCustomValue = "ProductPage_11",
                                        Product = new Product
                                        {
                                            ProductName = "Product_43",
                                            ProductCustomValue = "Product_Promocion_2",
                                            ProductImage = "~/Content/rsc/imgs/prueba.png",
                                            ProductOrder = 2,
                                            ProductURL = "",
                                            ProductSubtitle = "Cleasing Toner"
                                        },
                                        ProductPageBgColor = "#4bc8b6",
                                        ProductPageTextTitle = "Botanics",
                                        ProductPageColorTextTitle = "#2b7b6f",
                                        ProductPageTextDescription1 = "Botanics Cleansing toner all bright",
                                        ProductPageTextDescription2 =
                                            "Contrarresta la deshidratación de tu piel. Para todo tipo de piel",
                                        ProductPageTextCharacteristic1 = "limpia y purifica tu piel",
                                        ProductPageTextCharacteristic2 =
                                            "Contiene karité, aceite de macadamia y malvavisco.",
                                        ProductPageColorTextDescription1 = "#ffffff",
                                        ProductPageColorTextDescription2 = "#ffffff",
                                        ProductPageColorTextCharacteristic1 = "#ffffff",
                                        ProductPageColorTextCharacteristic2 = "#ffffff"
                                    },
                                    new ProductPage
                                    {
                                        ProductPageActive = "Activada",
                                        ProductPageCreatedDate = DateTime.Now,
                                        ProductPageCustomValue = "ProductPage_12",
                                        Product = new Product
                                        {
                                            ProductName = "Product_44",
                                            ProductCustomValue = "Product_Promocion_2",
                                            ProductImage = "~/Content/rsc/imgs/prueba.png",
                                            ProductOrder = 3,
                                            ProductURL = "",
                                            ProductSubtitle = "Cleasing Toner"
                                        },
                                        ProductPageBgColor = "#4bc8b6",
                                        ProductPageTextTitle = "Botanics",
                                        ProductPageColorTextTitle = "#2b7b6f",
                                        ProductPageTextDescription1 = "Botanics Cleansing toner all bright",
                                        ProductPageTextDescription2 =
                                            "Contrarresta la deshidratación de tu piel. Para todo tipo de piel",
                                        ProductPageTextCharacteristic1 = "limpia y purifica tu piel",
                                        ProductPageTextCharacteristic2 =
                                            "Contiene karité, aceite de macadamia y malvavisco.",
                                        ProductPageColorTextDescription1 = "#ffffff",
                                        ProductPageColorTextDescription2 = "#ffffff",
                                        ProductPageColorTextCharacteristic1 = "#ffffff",
                                        ProductPageColorTextCharacteristic2 = "#ffffff"
                                    },
                                    new ProductPage
                                    {
                                        ProductPageActive = "Activada",
                                        ProductPageCreatedDate = DateTime.Now,
                                        ProductPageCustomValue = "ProductPage_13",
                                        Product = new Product
                                        {
                                            ProductName = "Product_45",
                                            ProductCustomValue = "Product_Promocion_2",
                                            ProductImage = "~/Content/rsc/imgs/prueba.png",
                                            ProductOrder = 4,
                                            ProductURL = "",
                                            ProductSubtitle = "Cleasing Toner"
                                        },
                                        ProductPageBgColor = "#4bc8b6",
                                        ProductPageTextTitle = "Botanics",
                                        ProductPageColorTextTitle = "#2b7b6f",
                                        ProductPageTextDescription1 = "Botanics Cleansing toner all bright",
                                        ProductPageTextDescription2 =
                                            "Contrarresta la deshidratación de tu piel. Para todo tipo de piel",
                                        ProductPageTextCharacteristic1 = "limpia y purifica tu piel",
                                        ProductPageTextCharacteristic2 =
                                            "Contiene karité, aceite de macadamia y malvavisco.",
                                        ProductPageColorTextDescription1 = "#ffffff",
                                        ProductPageColorTextDescription2 = "#ffffff",
                                        ProductPageColorTextCharacteristic1 = "#ffffff",
                                        ProductPageColorTextCharacteristic2 = "#ffffff"
                                    },
                                    new ProductPage
                                    {
                                        ProductPageActive = "Activada",
                                        ProductPageCreatedDate = DateTime.Now,
                                        ProductPageCustomValue = "ProductPage_14",
                                        Product = new Product
                                        {
                                            ProductName = "Product_46",
                                            ProductCustomValue = "Product_Promocion_2",
                                            ProductImage = "~/Content/rsc/imgs/prueba.png",
                                            ProductOrder = 5,
                                            ProductURL = "",
                                            ProductSubtitle = "Cleasing Toner"
                                        },
                                        ProductPageBgColor = "#4bc8b6",
                                        ProductPageTextTitle = "Botanics",
                                        ProductPageColorTextTitle = "#2b7b6f",
                                        ProductPageTextDescription1 = "Botanics Cleansing toner all bright",
                                        ProductPageTextDescription2 =
                                            "Contrarresta la deshidratación de tu piel. Para todo tipo de piel",
                                        ProductPageTextCharacteristic1 = "limpia y purifica tu piel",
                                        ProductPageTextCharacteristic2 =
                                            "Contiene karité, aceite de macadamia y malvavisco.",
                                        ProductPageColorTextDescription1 = "#ffffff",
                                        ProductPageColorTextDescription2 = "#ffffff",
                                        ProductPageColorTextCharacteristic1 = "#ffffff",
                                        ProductPageColorTextCharacteristic2 = "#ffffff"
                                    },
                                    new ProductPage
                                    {
                                        ProductPageActive = "Activada",
                                        ProductPageCreatedDate = DateTime.Now,
                                        ProductPageCustomValue = "ProductPage_15",
                                        Product = new Product
                                        {
                                            ProductName = "Product_47",
                                            ProductCustomValue = "Product_Promocion_2",
                                            ProductImage = "~/Content/rsc/imgs/prueba.png",
                                            ProductOrder = 6,
                                            ProductURL = "",
                                            ProductSubtitle = "Cleasing Toner"
                                        },
                                        ProductPageBgColor = "#4bc8b6",
                                        ProductPageTextTitle = "Botanics",
                                        ProductPageColorTextTitle = "#2b7b6f",
                                        ProductPageTextDescription1 = "Botanics Cleansing toner all bright",
                                        ProductPageTextDescription2 =
                                            "Contrarresta la deshidratación de tu piel. Para todo tipo de piel",
                                        ProductPageTextCharacteristic1 = "limpia y purifica tu piel",
                                        ProductPageTextCharacteristic2 =
                                            "Contiene karité, aceite de macadamia y malvavisco.",
                                        ProductPageColorTextDescription1 = "#ffffff",
                                        ProductPageColorTextDescription2 = "#ffffff",
                                        ProductPageColorTextCharacteristic1 = "#ffffff",
                                        ProductPageColorTextCharacteristic2 = "#ffffff"
                                    },
                                    new ProductPage
                                    {
                                        ProductPageActive = "Activada",
                                        ProductPageCreatedDate = DateTime.Now,
                                        ProductPageCustomValue = "ProductPage_16",
                                        Product = new Product
                                        {
                                            ProductName = "Product_48",
                                            ProductCustomValue = "Product_Promocion_2",
                                            ProductImage = "~/Content/rsc/imgs/prueba.png",
                                            ProductOrder = 7,
                                            ProductURL = "",
                                            ProductSubtitle = "Cleasing Toner"
                                        },
                                        ProductPageBgColor = "#4bc8b6",
                                        ProductPageTextTitle = "Botanics",
                                        ProductPageColorTextTitle = "#2b7b6f",
                                        ProductPageTextDescription1 = "Botanics Cleansing toner all bright",
                                        ProductPageTextDescription2 =
                                            "Contrarresta la deshidratación de tu piel. Para todo tipo de piel",
                                        ProductPageTextCharacteristic1 = "limpia y purifica tu piel",
                                        ProductPageTextCharacteristic2 =
                                            "Contiene karité, aceite de macadamia y malvavisco.",
                                        ProductPageColorTextDescription1 = "#ffffff",
                                        ProductPageColorTextDescription2 = "#ffffff",
                                        ProductPageColorTextCharacteristic1 = "#ffffff",
                                        ProductPageColorTextCharacteristic2 = "#ffffff"
                                    },
                                    new ProductPage
                                    {
                                        ProductPageActive = "Activada",
                                        ProductPageCreatedDate = DateTime.Now,
                                        ProductPageCustomValue = "ProductPage_17",
                                        Product = new Product
                                        {
                                            ProductName = "Product_49",
                                            ProductCustomValue = "Product_Promocion_2",
                                            ProductImage = "~/Content/rsc/imgs/prueba.png",
                                            ProductOrder = 8,
                                            ProductURL = "",
                                            ProductSubtitle = "Cleasing Toner"
                                        },
                                        ProductPageBgColor = "#4bc8b6",
                                        ProductPageTextTitle = "Botanics",
                                        ProductPageColorTextTitle = "#2b7b6f",
                                        ProductPageTextDescription1 = "Botanics Cleansing toner all bright",
                                        ProductPageTextDescription2 =
                                            "Contrarresta la deshidratación de tu piel. Para todo tipo de piel",
                                        ProductPageTextCharacteristic1 = "limpia y purifica tu piel",
                                        ProductPageTextCharacteristic2 =
                                            "Contiene karité, aceite de macadamia y malvavisco.",
                                        ProductPageColorTextDescription1 = "#ffffff",
                                        ProductPageColorTextDescription2 = "#ffffff",
                                        ProductPageColorTextCharacteristic1 = "#ffffff",
                                        ProductPageColorTextCharacteristic2 = "#ffffff"
                                    },
                                    new ProductPage
                                    {
                                        ProductPageActive = "Activada",
                                        ProductPageCreatedDate = DateTime.Now,
                                        ProductPageCustomValue = "ProductPage_18",
                                        Product = new Product
                                        {
                                            ProductName = "Product_50",
                                            ProductCustomValue = "Product_Promocion_2",
                                            ProductImage = "~/Content/rsc/imgs/prueba.png",
                                            ProductOrder = 9,
                                            ProductURL = "",
                                            ProductSubtitle = "Cleasing Toner"
                                        },
                                        ProductPageBgColor = "#4bc8b6",
                                        ProductPageTextTitle = "Botanics",
                                        ProductPageColorTextTitle = "#2b7b6f",
                                        ProductPageTextDescription1 = "Botanics Cleansing toner all bright",
                                        ProductPageTextDescription2 =
                                            "Contrarresta la deshidratación de tu piel. Para todo tipo de piel",
                                        ProductPageTextCharacteristic1 = "limpia y purifica tu piel",
                                        ProductPageTextCharacteristic2 =
                                            "Contiene karité, aceite de macadamia y malvavisco.",
                                        ProductPageColorTextDescription1 = "#ffffff",
                                        ProductPageColorTextDescription2 = "#ffffff",
                                        ProductPageColorTextCharacteristic1 = "#ffffff",
                                        ProductPageColorTextCharacteristic2 = "#ffffff"
                                    },
                                }
                            },
                            new PromocionPage
                            {
                                PromocionPageColorHeadBg = "#ff9194",
                                PromocionPageImageLogo1 = "~/Content/rsc/imgs/foxe-logo.png",
                                PromocionPageImageLogo2 = "~/Content/rsc/imgs/foxe-producto.png",
                                PromocionPageHeadText = "Perfección a tu alcance",
                                PromocionPageSpanHeadText = "a tu alcance",
                                PromocionPageSubText1 = "desde Inglaterra a tu farmacia",
                                PromocionPageSubText2 = "NUESTROS PRODUCTOS EXCLUSIVOS",
                                PromocionPageSpanHeadtextColor = "#fed800",
                                PromocionPageHeadtextColor = "#ffffff",
                                PromocionPageActive = "Activada",
                                PromocionPageCreatedDate = DateTime.Now,
                                PromocionPageCustomValue = "Promocion_3",
                                PromocionPageHeadImage = "~/Content/rsc/imgs/promo-3.jpg",
                                PromocionPageOrder = 3,
                            }
                        }
                    }
                };
                fosePages.ForEach(a => context.FosePages.AddOrUpdate(h => h.FosePageCustomValue, a));
                context.SaveChanges();

                var imageSections = new List<ImageSection>
                {
                    new ImageSection
                    {
                        ImageSectionImage = "~/Content/rsc/imgs/banner_04.png",
                        ImageSectionPageId = fosePages[0].FosePageId,
                        ImageSectionPageName = "FosePage",
                        ImageSectionActive = true
                    }
                };
                imageSections.ForEach(a => context.ImageSections.AddOrUpdate(a));
                context.SaveChanges();
            }

            return FoseSections;
        }

        private static void saveBranchPages(MyApplicationDbContext context)
        {
            if (!context.BranchPages.Any())
            {
                var branchPages = new List<BranchPage>
                {
                    new BranchPage
                    {
                        BranchPageCustomValue = "BranchPage_1",
                        BranchPageTitle = "Sucursales",
                        BranchPageMessage = "¿Dónde te encuentras?",
                        BranchPageBranchNames = "Sucursales 24 hrs",
                        BranchPageActive = true,
                        BranchPageCreatedDate = DateTime.Now,
                        BranchPageColorMessage = "#2a3479",
                        BranchPageColorTextBranchNames = "#48c7f3",
                    }
                };
                branchPages.ForEach(a => context.BranchPages.AddOrUpdate(h => h.BranchPageCustomValue, a));
                context.SaveChanges();

                var imageSections = new List<ImageSection>
                {
                    new ImageSection
                    {
                        ImageSectionImage = "~/Content/rsc/imgs/banner_03.png",
                        ImageSectionPageId = branchPages[0].BranchPageId,
                        ImageSectionPageName = "BranchPage",
                        ImageSectionActive = true
                    },
                };
                imageSections.ForEach(a => context.ImageSections.AddOrUpdate(a));
                context.SaveChanges();
            }
        }

        private static void saveBillingPages(MyApplicationDbContext context, List<BenefitSection> benefitSections, List<IncrementBenefitSection> incrementBenefitSections)
        {
            if (!context.BillingPages.Any())
            {
                if (!context.LabSections.Any())
                {
                    var labSections = new List<LabSection>
                    {
                        new LabSection
                        {
                            LabSectionCustomValue = "LabSection_1",
                            LabSectionTitle = "Laboratorios participantes"
                        }
                    };
                    labSections.ForEach(a => context.LabSections.AddOrUpdate(h => h.LabSectionCustomValue, a));
                    context.SaveChanges();

                    var billingPages = new List<BillingPage>
                    {
                        new BillingPage
                        {
                            BillingPageCustomValue = "BillingPage_1",
                            BillingPageTitle = "Facturación en línea",
                            BillingPageActive = true,
                            BillingPageCreatedDate = DateTime.Now,
                            BenefitSectionId =
                                benefitSections.Single(g => g.BenefitSectionCustomValue == "BenefitSection_1")
                                    .BenefitSectionId,
                            IncrementBenefitSectionId =
                                incrementBenefitSections.Single(
                                    g => g.IncrementBenefitSectionCustomValue == "IncrementBenefitSection_1")
                                    .IncrementBenefitSectionId,
                            LabSectionId = labSections[0].LabSectionId
                        }
                    };
                    billingPages.ForEach(a => context.BillingPages.AddOrUpdate(h => h.BillingPageCustomValue, a));
                    context.SaveChanges();

                    var imageSections = new List<ImageSection>
                    {
                        new ImageSection
                        {
                            ImageSectionImage = "~/Content/rsc/imgs/banner_02.png",
                            ImageSectionPageId = billingPages[0].BillingPageId,
                            ImageSectionPageName = "BillingPage",
                            ImageSectionActive = true
                        },
                        new ImageSection
                        {
                            ImageSectionImage = "~/Content/rsc/imgs/laboratorios_participantes.png",
                            ImageSectionPageId = labSections[0].LabSectionId,
                            ImageSectionPageName = "LabSection",
                            ImageSectionActive = true
                        },
                        new ImageSection
                        {
                            ImageSectionImage = "~/Content/rsc/imgs/laboratorios_participantes.png",
                            ImageSectionPageId = labSections[0].LabSectionId,
                            ImageSectionPageName = "LabSection",
                            ImageSectionActive = true
                        },
                    };
                    imageSections.ForEach(a => context.ImageSections.AddOrUpdate(a));
                    context.SaveChanges();
                }
            }
        }

        private static void saveOfferPages(MyApplicationDbContext context, List<OfferType> offerTypes)
        {
            if (!context.OfferPages.Any())
            {
                var offerPages = new List<OfferPage>
                {
                    new OfferPage
                    {
                        OfferPageCustomValue = "OfferPage_Lunes_1",
                        OfferPageTitle = "Ben Alivio",
                        OfferPageActive = true,
                        OfferPageCreatedDate = DateTime.Now,
                        OfferTypeId = offerTypes[0].OfferTypeId,
                        OfferImage = "~/Content/rsc/imgs/ofertas.png",
                        OfferPageFillColor = "#48c7f3",
                        OfferPageText1 = "encuentra",
                        OfferPageColorText1 = "#ffffff",
                        OfferPageTextType1 = "md",
                        OfferPageText2 = "benalivio",
                        OfferPageColorText2 = "#ffffff",
                        OfferPageSpan2 = "alivio",
                        OfferPageColorSpan2 = "#2a3479",
                        OfferPageTextType2 = "lg",
                        OfferPageText3 = "vigencia: 10 de abril de 2017",
                        OfferPageColorText3 = "#ffffff",
                        OfferPageTextType3 = "xs",
                    },
                    new OfferPage
                    {
                        OfferPageCustomValue = "OfferPage_Miercoles_1",
                        OfferPageTitle = "Mierconómicos",
                        OfferPageActive = true,
                        OfferPageCreatedDate = DateTime.Now,
                        OfferTypeId = offerTypes[1].OfferTypeId,
                        OfferImage = "~/Content/rsc/imgs/ofertas.png",
                        OfferPageFillColor = "#ec2028",
                        OfferPageText1 = "mierconómicos",
                        OfferPageColorText1 = "#ffffff",
                        OfferPageTextType1 = "lg",
                        OfferPageText2 = "ofertas de mitad de semana",
                        OfferPageColorText2 = "#ffffff",
                        OfferPageTextType2 = "md",
                        OfferPageText3 = "vigencia: 10 de abril de 2017",
                        OfferPageColorText3 = "#ffffff",
                        OfferPageTextType3 = "xs",
                    },
                    new OfferPage
                    {
                        OfferPageCustomValue = "OfferPage_Viernes_1",
                        OfferPageTitle = "Ben Ahorro",
                        OfferPageActive = true,
                        OfferPageCreatedDate = DateTime.Now,
                        OfferTypeId = offerTypes[2].OfferTypeId,
                        OfferImage = "~/Content/rsc/imgs/ofertas.png",
                        OfferPageFillColor = "#48c7f3",
                        OfferPageText1 = "benahorro$",
                        OfferPageColorText1 = "#ffffff",
                        OfferPageSpan1 = "ahorro",
                        OfferPageColorSpan1 = "#2a3479",
                        OfferPageTextType1 = "lg",
                        OfferPageText2 = "vigencia: 10 de abril de 2017",
                        OfferPageColorText2 = "#ffffff",
                        OfferPageTextType2 = "xs",
                    },
                    new OfferPage
                    {
                        OfferPageCustomValue = "OfferPage_Mensuales_1",
                        OfferPageTitle = "Ben Ahorro",
                        OfferPageActive = true,
                        OfferPageCreatedDate = DateTime.Now,
                        OfferTypeId = offerTypes[3].OfferTypeId,
                        OfferImage = "~/Content/rsc/imgs/ofertas.png",
                        OfferPageFillColor = "#48c7f3",
                        OfferPageText1 = "benahorro$",
                        OfferPageColorText1 = "#ffffff",
                        OfferPageSpan1 = "ahorro",
                        OfferPageColorSpan1 = "#2a3479",
                        OfferPageTextType1 = "lg",
                        OfferPageText2 = "vigencia: 10 de abril de 2017",
                        OfferPageColorText2 = "#ffffff",
                        OfferPageTextType2 = "xs",
                    },
                    new OfferPage
                    {
                        OfferPageCustomValue = "OfferPage_Especiales_1",
                        OfferPageTitle = "Ben Ahorro",
                        OfferPageActive = true,
                        OfferPageCreatedDate = DateTime.Now,
                        OfferTypeId = offerTypes[4].OfferTypeId,
                        OfferImage = "~/Content/rsc/imgs/ofertas.png",
                        OfferPageFillColor = "#48c7f3",
                        OfferPageText1 = "benahorro$",
                        OfferPageColorText1 = "#ffffff",
                        OfferPageSpan1 = "ahorro",
                        OfferPageColorSpan1 = "#2a3479",
                        OfferPageTextType1 = "lg",
                        OfferPageText2 = "vigencia: 10 de abril de 2017",
                        OfferPageColorText2 = "#ffffff",
                        OfferPageTextType2 = "xs",
                    }
                };
                offerPages.ForEach(a => context.OfferPages.AddOrUpdate(h => h.OfferPageCustomValue, a));
                context.SaveChanges();
            }
        }

        private static List<IncrementBenefitSection> saveIncrementBenefitSections(MyApplicationDbContext context)
        {
            var incrementBenefitSections = new List<IncrementBenefitSection>
            {
                new IncrementBenefitSection
                {
                    IncrementBenefitSectionCustomValue = "IncrementBenefitSection_1",
                    IncrementBenefitSectionImage1 = "~/Content/rsc/imgs/platino.png",
                    IncrementBenefitSectionImage2 = "~/Content/rsc/imgs/platino_oferta.png",
                    IncrementBenefitSectionTitle = "",
                    IncrementBenefitSectionDiv = "<h1 class=\"weight-heavy full-width\"><span class=\"color-red\">aumenta </span><span class=\"color-blue\">tus beneficios</span></h1><h4 class=\"weight-heavy color-blue full-width\">con mi tarjeta<span class=\"color-red\"> Benavides Platino </span>los beneficios se multiplican</h4><br><ol class=\"numbered-list\"><li>Obsequio de bienvenida</li><li>Dinero electrónico*: En todas tus compras acumulas dinero electrónico y en marcas propias acumulas el 10%</li><li>Descuento para +55 años: obtén 7% de descuento los primeros 7 días del mes y 5% el resto</li><li><a href = \"javascript:void(0)\" id= \"laboratories\" > Medicamentos gratis en más de 35 laboratorios:*</a></li><li>Acceso exclusivo a<a href=\"\" class=\"color-blue\">Club Peques</a></li><li>Consulta médica gratuita: 1 consulta médica gratuita al año a partir de la activación o renovación de tu tarjeta</li><li>Seguro de vida: seguro de vida por muerte accidental por una suma asegurada de hasta $210,000. Vigencia de 1 año para el seguro. <br><ul class=\"nested-list\"><li>Beneficio otorgado/operado y a cargo de ACE Seguros, S.A.<ul class=\"nested-list\"><li>Aplica en caso de muerte accidental, incluso en caso de asaltos.</li><li>Activación automática del seguro al comprar la tarjeta y registrarse en sucursal.</li><li>Podrás designar a tu beneficiario contactando a ACE Seguros</li><li>Para la reclamación del seguro, el beneficiario debe presentar la tarjeta y demostrar la relación.</li><li>Dudas e información, comunícate a ACE Seguros en el teléfono 01 800 237 6266 o en la página www.acegroup.com/mx-es consulta también el aviso de privacidad de la seguradora en dicha página.</li></ul></li></ul></li><li>Seguro de ambulancia: 2 ambulancias al año gratis llamando al 01 800 237 6266. <br><ul class=\"nested-list\"><li>Beneficio otorgado/operado y a cargo de ACE Seguros, S.A.<ul class=\"nested-list\"><li>Para uso exclusivo del titular de la tarjeta.</li><li>Únicamente en caso de emergencia, aplican restricciones.</li><li>Programa a nivel nacional las 24 horas y 365 días del año.</li><li>Traslado al hospital más cercano.</li><li>Equipada con todos los servicios básicos de primeros auxilios y operada por paramédicos certificados.</li><li>Envío de ambulancia por emergencia.Si el beneficiario o consecuencia de accidente o enfermedad repentina que le provoque lesiones o traumatismos que pongan en peligro su vida, se gestionará su traslado al centro hospitalario más cercano y/o adecuado. Si fuera necesario por razones médicas, se realizará el traslado bajo supervisión médica, mediante ambulancia terrestre, de terapia intensiva, intermedia o estándar, dependiendo de la gravedad y circunstancias de cada caso.</li></ul></li></ul><br><br>Orientación telefónica: Obtén orientación telefónica llamando al 01 800 237 6266 <br><br>Descuentos en laboratorios de análisis clínicos (<a data-toggle= \"modal\" href= \"#modal-ciudades\" class=\"color-blue\">consulta el listado de ciudades participantes</a>) 3% de descuento en tus compras(*Excepto en Tabaquería, alcohol, raspacitos, pagos de servicios, fórmulas maternizadas, pañales, agua envasada, bebidas y refrescos, alimentos funcionales, artículos que estén en acumulación de piezas, productos que tengan otra oferta/promoción, y medicamentos de especialidad)</li></ol><p class=\"padded-top color-blue small\">Excepciones de acumulación de dinero electrónico: Tabaquería, alcohol, recargas telefónicas, raspacitos, pagos de servicios, fórmulas maternizadas, medicamentos de especialidad y controlados.</p><br>",
                }
            };
            incrementBenefitSections.ForEach(
                a => context.IncrementBenefitSections.AddOrUpdate(h => h.IncrementBenefitSectionCustomValue, a));
            context.SaveChanges();
            return incrementBenefitSections;
        }

        private static List<BenefitSection> saveBenefitSections(MyApplicationDbContext context)
        {
            var benefitSections = new List<BenefitSection>
            {
                new BenefitSection
                {
                    BenefitSectionCustomValue = "BenefitSection_1",
                    BenefitSectionImage = "~/Content/rsc/imgs/tarjeta.png",
                    BenefitSectionImageXS = "~/Content/rsc/imgs/tarjeta-xs.png",
                    BenefitSectionTitle = "",
                    BenefitSectionDiv =
                        "<h1 class=\"weight-heavy full-width\"><span class=\"color-blue\">usa tu </span><span class=\"color-blue-marine\">tarjeta Benavides</span></h1>" +
                        "<h4 class=\"weight-heavy color-blue full-width\">con ella obtienes beneficios en todas tus compras</h4><br>" +
                        "<p class=\"color-blue\">Con tu tarjeta Benavides obtienes grandes beneficios y dinero electrónico. Preséntala en cada compra que realices en todas nuestras sucursales.</p><br>" +
                        "<ol class=\"numbered-list\">" +
                        "<li>Obsequio de bienvenida</li>" +
                        "<li>Dinero electrónico*: En todas tus compras acumulas dinero electrónico y en marcas propias acumulas el 10%</li>" +
                        "<li>Descuento para +55 años: obtén 7% de descuento los primeros 7 días del mes y 5% el resto</li>" +
                        "<li>Medicamentos gratis en más de 35 laboratorios:*</li>" +
                        "<li>Acceso exclusivo a club peques: te manda al registro</li>" +
                        "</ol>",
                    BenefitSectionParagraph =
                        "<span class=\"weight-heavy\">Adicional a estos beneficios las personas mayores de 55 años reciben el 7% de descuentos los primeros 7 días del mes y 5% de descuento el resto.</span><br>" +
                        "La tarjeta Benavides es la única tarjeta que tiene convenio con más de 40 laboratorios para que tú recibas más de 1,000 medicamentos gratis para padecimientos crónicos como diabetes, hipertensión, gastrointestinal, obesidad, depresión, entre otros."
                }
            };
            benefitSections.ForEach(a => context.BenefitSections.AddOrUpdate(h => h.BenefitSectionCustomValue, a));
            context.SaveChanges();
            return benefitSections;
        }

        private static List<CardSection> saveCardSections(MyApplicationDbContext context)
        {
            var cardSections = new List<CardSection>
            {
                new CardSection
                {
                    CardSectionCustomValue = "CardSection_1",
                    CardSectionTitle = "Tarjeta Benavides",
                    CardSectionColorTitle = "#2a3479",
                    CardSectionImage = "~/Content/rsc/imgs/BI.png",
                    CardSectionImageXS = "~/Content/rsc/imgs/BI_xs.png"
                }
            };
            cardSections.ForEach(a => context.CardSections.AddOrUpdate(h => h.CardSectionCustomValue, a));
            context.SaveChanges();
            return cardSections;
        }

        private static DoctorsOfficeSection saveDoctorsOfficeSection(MyApplicationDbContext context)
        {
            DoctorsOfficeSection result = new DoctorsOfficeSection()
            {
                BackgroundColor = "#04559f",
                ImageFileName = "~/Content/rsc/imgs/doctors-office-recipe.png",
                LogoImageFileName = "~/Content/rsc/imgs/doctors-office-logo.png",
                Link = "~/Home",
                SectionMessageText = "Tu médico de confianza",
                Title = "Consultorios Médicos",
                SectionMessageTextColor = "#c8e9fb",
                TitleColor = "#2a3479"
            };

            /*context.DoctorsOfficeSections.Add(result);
            context.SaveChanges();*/
            return result;
        }

        private static Models.Pages.Sections.FourQuadSection saveFourQuadSection(MyApplicationDbContext context)
        {
            Models.Pages.Sections.FourQuadSection result = new Models.Pages.Sections.FourQuadSection() {
                Quad1 = new Models.Pages.Sections.SingleQuadSectionItem() {
                    Text = "¿quiénes somos?",
                    Link = "~/WhoWeAre",
                    BackgroundColor = "#45b9e6",
                    ImageFileName = "~/Content/rsc/imgs/quienes-somos.png"
                },
                Quad2 = new Models.Pages.Sections.SingleQuadSectionItem() {
                    Text = "responsabilidad social",
                    Link = "~/Home",
                    BackgroundColor = "#a9defb",
                    ImageFileName = "~/Content/rsc/imgs/responsabilidad.png"
                },
                Quad3 = new Models.Pages.Sections.SingleQuadSectionItem() {
                    Text = "catálogo",
                    Link = "~/UploadedFiles/catalog.pdf",
                    BackgroundColor = "#45b9e6",
                    ImageFileName = "~/Content/rsc/imgs/consultorios.png"
                },
                Quad4 = new Models.Pages.Sections.SingleQuadSectionItem() {
                    Text = "¿terreno o local disponible?",
                    Link = "mailto:inmobiliaria@benavides.com.mx",
                    BackgroundColor = "#a9defb",
                    ImageFileName = "~/Content/rsc/imgs/terrenos.png"
                }
            };

            context.FourQuadSections.Add(result);
            //context.SaveChanges();
            return result;
        }

        private static List<BlogSection> saveBlogSections(MyApplicationDbContext context)
        {
            var blogSections = new List<BlogSection>
            {
                new BlogSection
                {
                    BlogSectionCustomValue = "BlogSection_1",
                    BlogSectionTitle = "Blog",
                    BlogSectionColorTitle = "#ec2028"
                }
            };
            blogSections.ForEach(a => context.BlogSections.AddOrUpdate(h => h.BlogSectionCustomValue, a));
            context.SaveChanges();

            var titleTypes = new List<TitleType>
            {
                new TitleType { TitleTypeMessage = "mi belleza", TitleTypeCustomValue = "TitleType_6", TitleTypeColor="#2a3479", TitleTypeHoverColor="#48c7f3", TitleTypeBgColor="#e0f3fd"  },
                new TitleType { TitleTypeMessage = "mi salud", TitleTypeCustomValue = "TitleType_7", TitleTypeColor="#2a3479", TitleTypeHoverColor="#48c7f3", TitleTypeBgColor="#c8e9fb"  },
                new TitleType { TitleTypeMessage = "mi bebé", TitleTypeCustomValue = "TitleType_8", TitleTypeColor="#2a3479", TitleTypeHoverColor="#48c7f3", TitleTypeBgColor="#e0f3fd"  },
                new TitleType { TitleTypeMessage = "mis gustos", TitleTypeCustomValue = "TitleType_9", TitleTypeColor="#2a3479", TitleTypeHoverColor="#48c7f3", TitleTypeBgColor="#c8e9fb"  }
            };
            titleTypes.ForEach(a => context.TitleTypes.AddOrUpdate(h => h.TitleTypeCustomValue, a));
            context.SaveChanges();

            var products = new List<Product>
            {
                 new Product
                 {
                    ProductName = "Product_6",
                    ProductCustomValue = "Product_BlogSection_HomPage_1",
                    ProductTitle_TitleTypeId = titleTypes[0].TitleTypeId,
                    ProductImage = "~/Content/rsc/imgs/belleza.png",
                    ProductOrder = 1,
                    BlogTypeId = 1,
                    ProductURL = "", 
                    BlogSectionId = 1,
                 },
                 new Product
                 {
                     ProductName = "Product_7",
                     ProductCustomValue = "Product_BlogSection_HomPage_1",
                     ProductTitle_TitleTypeId = titleTypes[1].TitleTypeId,
                     ProductImage = "~/Content/rsc/imgs/salud.png",
                     ProductOrder = 2,
                     BlogTypeId = 2,
                     ProductURL = "",
                     BlogSectionId = 1,
                 },
                 new Product
                 {
                    ProductName = "Product_8",
                    ProductCustomValue = "Product_BlogSection_HomPage_1",
                    ProductTitle_TitleTypeId = titleTypes[2].TitleTypeId,
                    ProductImage = "~/Content/rsc/imgs/bebe.png",
                    ProductOrder = 3,
                    BlogTypeId = 3,
                    ProductURL = "",
                    BlogSectionId = 1,
                 },
                 new Product
                 {
                    ProductName = "Product_9",
                    ProductCustomValue = "Product_BlogSection_HomPage_1",
                    ProductTitle_TitleTypeId = titleTypes[3].TitleTypeId,
                    ProductImage = "~/Content/rsc/imgs/gustos.png",
                    ProductOrder = 4,
                    BlogTypeId = 4,
                    ProductURL = "",
                    BlogSectionId = 1,
                 },
            };
            products.ForEach(a => context.Products.AddOrUpdate(h => h.ProductName, a));
            context.SaveChanges();

            return blogSections;
        }

        private static List<OfferSection> saveOfferSections(MyApplicationDbContext context)
        {
            var offerSections = new List<OfferSection>
            {
                new OfferSection
                {
                    OfferSectionCustomValue = "OfferSection_1",
                    OfferSectionTitle = "Siéntete consentido",
                    OfferSectionColorTitle = "#ec2028"
                }
            };
            offerSections.ForEach(a => context.OfferSections.AddOrUpdate(h => h.OfferSectionCustomValue, a));
            context.SaveChanges();

            var titleTypes = new List<TitleType>
            {
                new TitleType { TitleTypeMessage = "benalivio$", TitleTypeSpan = "alivio", TitleTypeCustomValue = "TitleType_1", TitleTypeColor="#2a3479" },
                new TitleType { TitleTypeMessage = "mierconómicos", TitleTypeSpan = "mierco", TitleTypeCustomValue = "TitleType_2", TitleTypeColor="#2a3479"  },
                new TitleType { TitleTypeMessage = "benahorro", TitleTypeSpan = "ahorro", TitleTypeCustomValue = "TitleType_3", TitleTypeColor="#2a3479"  },
                new TitleType { TitleTypeMessage = "benalivio$", TitleTypeSpan = "alivio", TitleTypeCustomValue = "TitleType_4", TitleTypeColor="#810b10"  },
                new TitleType { TitleTypeMessage = "ofertas especiales", TitleTypeSpan = "ofertas", TitleTypeCustomValue = "TitleType_5", TitleTypeColor="#810b10"  },
            };
            titleTypes.ForEach(a => context.TitleTypes.AddOrUpdate(h => h.TitleTypeCustomValue, a));
            context.SaveChanges();

            var products = new List<Product>
            {
                new Product
                {
                    ProductName = "Product_1",
                    ProductCustomValue = "Product_OfferSection_HomPage_1",
                    ProductTitle_TitleTypeId = titleTypes[0].TitleTypeId,
                    ProductImage = "~/Content/rsc/imgs/vaporub.png",
                    OfferTypeId = 1,
                    ProductOrder = 1,
                    OfferSectionId = 1
                },
                new Product
                {
                    ProductName = "Product_2",
                    ProductCustomValue = "Product_OfferSection_HomPage_1",
                    ProductTitle_TitleTypeId = titleTypes[1].TitleTypeId,
                    ProductImage = "~/Content/rsc/imgs/koleston.png",
                    OfferTypeId = 2,
                    ProductOrder = 2,
                    OfferSectionId = 1
                 },
                 new Product
                 {
                    ProductName = "Product_3",
                    ProductCustomValue = "Product_OfferSection_HomPage_1",
                    ProductTitle_TitleTypeId = titleTypes[2].TitleTypeId,
                    ProductImage = "~/Content/rsc/imgs/nan.png",
                    OfferTypeId = 3,
                    ProductOrder = 3,
                    OfferSectionId = 1
                 },
                 new Product
                 {
                    ProductName = "Product_4",
                    ProductCustomValue = "Product_OfferSection_HomPage_1",
                    ProductTitle_TitleTypeId = titleTypes[3].TitleTypeId,
                    ProductImage = "~/Content/rsc/imgs/trojan.png",
                    OfferTypeId = 4,
                    ProductOrder = 4,
                    OfferSectionId = 1
                 },
                 new Product
                 {
                    ProductName = "Product_5",
                    ProductCustomValue = "Product_OfferSection_HomPage_1",
                    ProductTitle_TitleTypeId = titleTypes[4].TitleTypeId,
                    ProductImage = "~/Content/rsc/imgs/pañales.png",
                    OfferTypeId = 5,
                    ProductOrder = 5,
                    OfferSectionId = 1
                 },
            };
            products.ForEach(a => context.Products.AddOrUpdate(h => h.ProductName, a));
            context.SaveChanges();

            return offerSections;
        }

        private static List<BlogType> saveBlogTypes(MyApplicationDbContext context)
        {
            var blogTypes = new List<BlogType>
            {
                new BlogType
                {
                    BlogTypeActive = true,
                    BlogTypeName = "Belleza"
                },
                new BlogType
                {
                    BlogTypeActive = true,
                    BlogTypeName = "Salud"
                },
                new BlogType
                {
                    BlogTypeActive = true,
                    BlogTypeName = "Bebé"
                },
                new BlogType
                {
                    BlogTypeActive = true,
                    BlogTypeName = "Gustos"
                },
            };

            blogTypes.ForEach(a => context.BlogTypes.AddOrUpdate(h => h.BlogTypeName, a));
            context.SaveChanges();
            return blogTypes;
        }

        private static List<OfferType> saveOfferTypes(MyApplicationDbContext context)
        {
            var offerTypes = new List<OfferType>
            {
                new OfferType
                {
                    OfferTypeActive = true,
                    OfferTypeName = "Lunes"
                },
                new OfferType
                {
                    OfferTypeActive = true,
                    OfferTypeName = "Miércoles"
                },
                new OfferType
                {
                    OfferTypeActive = true,
                    OfferTypeName = "Viernes"
                },
                new OfferType
                {
                    OfferTypeActive = true,
                    OfferTypeName = "Especiales"
                },
                new OfferType
                {
                    OfferTypeActive = true,
                    OfferTypeName = "Mensuales"
                },
            };

            offerTypes.ForEach(a => context.OfferTypes.AddOrUpdate(h => h.OfferTypeName, a));
            context.SaveChanges();
            return offerTypes;
        }


        private static void saveBranchs(MyApplicationDbContext context)
        {
            #region old_branches
            /*
            var branchs = new List<Branch>
            {
                new Branch
                {
                    BranchRegion = "R2",
                    BranchCeco = "2L0899",
                    BranchSap = "M899",
                    BranchName = "Ocotlan (PH)",
                    BranchAddress = "Niños Héroes No. 263, entre Hidalgo y Morelos, Col. Centro, Ocotlán, Jalisco, C.P. 47800",
                    BranchCity = "Jalisco",
                    BranchHour1 = "8:00 AM",
                    BranchHour2 = "11:00 PM",
                    BranchConsult = false,
                    BranchLatitude = "20.3463",
                    BranchLongitude = "-102.7744",
                    BranchActive = true
                },
                new Branch
                {
                    BranchRegion = "R7",
                    BranchCeco = "2L1166",
                    BranchSap = "N166",
                    BranchName = "Montevideo (PH)",
                    BranchAddress =
                        "Av.  Montevideo 195, entre Pernambuco y Eje 5 Nte., Col. Lindavista Sur, Deleg. Gustavo A. Madero, Distrito Federal, C.P. 07300",
                    BranchCity = "DF",
                    BranchHour1 = "7:00 AM",
                    BranchHour2 = "11:00 PM",
                    BranchConsult = false,
                    BranchLatitude = "19.4890",
                    BranchLongitude = "-99.12811",
                    BranchActive = true
                },
                new Branch
                {
                    BranchRegion = "R2",
                    BranchCeco = "2L1240",
                    BranchSap = "N240",
                    BranchName = "Av. La Paz (PH)",
                    BranchAddress =
                        "Av. La Paz No. 1737, entre Juan de Ojeda y Emeterio Robles Gil, Col. Americana,  Guadalajara, Jalisco C.P. 49000",
                    BranchCity = "Guadalajara",
                    BranchHour1 = "8:00 AM",
                    BranchHour2 = "11:00 PM",
                    BranchConsult = false,
                    BranchLatitude = "20.671203",
                    BranchLongitude = "-103.363442",
                    BranchActive = true
                },
                new Branch
                {
                    BranchRegion = "R1",
                    BranchCeco = "2L0890",
                    BranchSap = "M890",
                    BranchName = "Francisco de Quevedo",
                    BranchAddress =
                        "Manuel L. Barragán No. 998 A, entre Francisco de Quevedo y Palacio de Justicia, Col. Anáhuac, San Nicolás de los Garza, N.L. C.P. 66450",
                    BranchCity = "Monterrey",
                    BranchHour1 = "12:00 AM",
                    BranchHour2 = "12:00 PM",
                    BranchConsult = true,
                    BranchLatitude = "25.7329",
                    BranchLongitude = "-100.3152",
                    BranchActive = true
                },
                new Branch
                {
                    BranchRegion = "R1",
                    BranchCeco = "2L1193",
                    BranchSap = "N193",
                    BranchName = "Plaza Once21",
                    BranchAddress =
                        "Av. Unión No. 100, esq. con Parque Industrial Fracc. Residencial Puerta del sol, Escobedo N.L. C.P. 66070",
                    BranchCity = "Monterrey",
                    BranchHour1 = "8:00 AM",
                    BranchHour2 = "11:00 PM",
                    BranchConsult = false,
                    BranchLatitude = "25.8032",
                    BranchLongitude = "-100.34135",
                    BranchActive = true
                },
                new Branch
                {
                    BranchRegion = "R5",
                    BranchCeco = "2L1194",
                    BranchSap = "N194",
                    BranchName = "Mesa Central",
                    BranchAddress =
                        "Custodio de la República No. 1205 Col. Manuel J. Clouthier, Cd. Juárez, Chihuahua, C.P. 32575",
                    BranchCity = "Chihuahua",
                    BranchHour1 = "8:00 AM",
                    BranchHour2 = "10:00 PM",
                    BranchConsult = false,
                    BranchLatitude = "31.6047",
                    BranchLongitude = "-106.3604",
                    BranchActive = true
                },
                new Branch
                {
                    BranchRegion = "R1",
                    BranchCeco = "2L1213",
                    BranchSap = "N213",
                    BranchName = "Mitras II (PH)",
                    BranchAddress = "Av. Simón Bolívar No. 1660, Col. Mitras Centro, Monterrey, N.L. C.P. 64460",
                    BranchCity = "Monterrey",
                    BranchHour1 = "7:00 AM",
                    BranchHour2 = "11:00 PM",
                    BranchConsult = false,
                    BranchLatitude = "25.6957",
                    BranchLongitude = "-100.3442",
                    BranchActive = true
                },
                new Branch
                {
                    BranchRegion = "R5",
                    BranchCeco = "2L1191",
                    BranchSap = "N191",
                    BranchName = "Cuatro Caminos (Cimaco)",
                    BranchAddress =
                        "Blvd. Independiencia No. 1300 Ote., entre Cuauhtémoc y Río Nazas, Col. Navarro, Torreón, Coahuila, C.P. 27010",
                    BranchCity = "Torreon",
                    BranchHour1 = "9:00 AM",
                    BranchHour2 = "9:00 PM",
                    BranchConsult = false,
                    BranchLatitude = "25.5594",
                    BranchLongitude = "-103.4341",
                    BranchActive = true
                },
                new Branch
                {
                    BranchRegion = "R7",
                    BranchCeco = "2L1192",
                    BranchSap = "N192",
                    BranchName = "05 de Mayo",
                    BranchAddress =
                        "Blvd. Héroes del 5 de Mayo No. 110, esquina con 3 Sur, Col. Huexotitla, Puebla, Puebla, C.P. 72534",
                    BranchCity = "Puebla",
                    BranchHour1 = "7:00 AM",
                    BranchHour2 = "11:00 PM",
                    BranchConsult = false,
                    BranchLatitude = "19.0265",
                    BranchLongitude = "-98.21149",
                    BranchActive = true
                },
                new Branch
                {
                    BranchRegion = "R1",
                    BranchCeco = "2L1201",
                    BranchSap = "N201",
                    BranchName = "Plaza Colibrí",
                    BranchAddress =
                        "Av. Revolución No.401 Local 1 cruz con Chapultepec, Col. Jardín Español, Monterrey, Nuevo León, C.P. 64820",
                    BranchCity = "Monterrey",
                    BranchHour1 = "12:00 AM",
                    BranchHour2 = "12:00 PM",
                    BranchConsult = true,
                    BranchLatitude = "25.664",
                    BranchLongitude = "-100.2860",
                    BranchActive = true
                },
                new Branch
                {
                    BranchRegion = "R7",
                    BranchCeco = "2L1307",
                    BranchSap = "N307",
                    BranchName = "Río Mayo",
                    BranchAddress = "Av. Río Mayo 301 esquina con Iguala, Col. Vista Hermosa, Cuernavaca, Morelos, C.P. 62290",
                    BranchCity = "Cuernavaca",
                    BranchHour1 = "7:00 AM",
                    BranchHour2 = "11:00 PM",
                    BranchConsult = false,
                    BranchLatitude = "18.9340",
                    BranchLongitude = "-99.2221",
                    BranchActive = true
                },
                new Branch
                {
                    BranchRegion = "R5",
                    BranchCeco = "2L0894",
                    BranchSap = "M894",
                    BranchName = "Fernando Baeza",
                    BranchAddress =
                        "Av. Fernando Baeza Oriente No. 208 entre Av. 7 Oriente y Plutarco Elías Calles, Fracc. Viñedos, Delicias Chihuahua, C.P. 33068",
                    BranchCity = "Delicias",
                    BranchHour1 = "7:00 AM",
                    BranchHour2 = "10:00 PM",
                    BranchConsult = false,
                    BranchLatitude = "28.18133",
                    BranchLongitude = "-105.4516",
                    BranchActive = true
                },
                new Branch
                {
                    BranchRegion = "R5",
                    BranchCeco = "2L1281",
                    BranchSap = "N281",
                    BranchName = "Portal de las Lomas",
                    BranchAddress =
                        "Blvd. Emilio Arizpe de la Maza No. 2335 esquina con Blvd. Militar, Col. Lomas del Sur, Saltillo, Coahuila de Zaragoza, C.P. 25084",
                    BranchCity = "Saltillo",
                    BranchHour1 = "8:00 AM",
                    BranchHour2 = "11:00 PM",
                    BranchConsult = false,
                    BranchLatitude = "25.3718",
                    BranchLongitude = "-101.0036",
                    BranchActive = true
                },
                new Branch
                {
                    BranchRegion = "R3",
                    BranchCeco = "2L1195",
                    BranchSap = "N195",
                    BranchName = "San Bernardino",
                    BranchAddress =
                        "Calzada de los Ángeles No. 516, Col. Alta California Residencial, Hermosillo, Sonora, C.P. 83249",
                    BranchCity = "Hermosillo",
                    BranchHour1 = "7:00 AM",
                    BranchHour2 = "11:00 PM",
                    BranchConsult = false,
                    BranchActive = true
                },
                new Branch
                {
                    BranchRegion = "R7",
                    BranchCeco = "2L1309",
                    BranchSap = "N309",
                    BranchName = "Plaza Diana",
                    BranchAddress = "Av. Diana No. 7, entre Galatea y Mesalina, Col. Delicias, Cuernavaca, Morelos, C.P. 62330",
                    BranchCity = "Cuernavaca",
                    BranchHour1 = "7:00 AM",
                    BranchHour2 = "11:00 PM",
                    BranchConsult = false,
                    BranchLatitude = "18.9362",
                    BranchLongitude = "-99.208899",
                    BranchActive = true
                },
                new Branch
                {
                    BranchRegion = "R5",
                    BranchCeco = "2L1308",
                    BranchSap = "N308",
                    BranchName = "El Porvenir (Gitanos)",
                    BranchAddress =
                        "Blvd. Manuel Gómez Morín No. 11632, entre Neptuno y Del Valle, Col. Partido Senecu, Cd. Juárez, Chihuahua, C.P. 32548",
                    BranchCity = "Cd. Juárez",
                    BranchHour1 = "8:00 AM",
                    BranchHour2 = "11:00 PM",
                    BranchConsult = false,
                    BranchLatitude = "31.68904",
                    BranchLongitude = "-106.3774",
                    BranchActive = true
                },
                new Branch
                {
                    BranchRegion = "R7",
                    BranchCeco = "2L1310",
                    BranchSap = "N310",
                    BranchName = "Mundo E",
                    BranchAddress =
                        "Blvd. A. Camacho No. 1007, entre Cuauhtémoc y Juárez, San Lucas Tepetlacalco, Tlalnepantla de Baz, Estado de México, C.P. 54055",
                    BranchCity = "DF",
                    BranchHour1 = "8:00 AM",
                    BranchHour2 = "11:00 PM",
                    BranchConsult = false,
                    BranchLatitude = "19.52568",
                    BranchLongitude = "-99.2287",
                    BranchActive = true
                },
                new Branch
                {
                    BranchRegion = "R7",
                    BranchCeco = "2L1316",
                    BranchSap = "N316",
                    BranchName = "Mirador",
                    BranchAddress = "39 Oriente No. 1806, esq. con 18 Sur, Fracc. El Mirador, Puebla, Puebla, C.P. 72530",
                    BranchCity = "Puebla",
                    BranchHour1 = "7:00 AM",
                    BranchHour2 = "11:00 PM",
                    BranchConsult = false,
                    BranchLatitude = "19.024677",
                    BranchLongitude = "-98.1931",
                    BranchActive = true
                },
                new Branch
                {
                    BranchRegion = "R3",
                    BranchCeco = "2L1272",
                    BranchSap = "N272",
                    BranchName = "5 Y 10",
                    BranchAddress =
                        "Blvd. Gustavo Díaz Ordaz No. 14730, Col. Gas y Anexas, Tijuana, Baja California, C.P. 22115",
                    BranchCity = "Tijuana",
                    BranchHour1 = "7:00 AM",
                    BranchHour2 = "12:00 PM",
                    BranchConsult = false,
                    BranchLatitude = "32.4970",
                    BranchLongitude = "-116.9628",
                    BranchActive = true
                },
                new Branch
                {
                    BranchRegion = "R5",
                    BranchCeco = "2L1305",
                    BranchSap = "N305",
                    BranchName = "Gardie",
                    BranchCity = "CD Juarez",
                    BranchHour1 = "",
                    BranchHour2 = "",
                    BranchConsult = false,
                    BranchLatitude = "31.737837",
                    BranchLongitude = "-106.426174",
                    BranchActive = true
                },
                new Branch
                {
                    BranchRegion = "R4",
                    BranchCeco = "2L1246",
                    BranchSap = "M246",
                    BranchName = "Mocambo",
                    BranchCity = "Veracruz",
                    BranchHour1 = "8:00 AM",
                    BranchHour2 = "11:00 PM",
                    BranchConsult = false,
                    BranchLatitude = "19.157130",
                    BranchLongitude = "-96.109127",
                    BranchActive = true
                },
                new Branch
                {
                    BranchRegion = "R7",
                    BranchCeco = "2L1314",
                    BranchSap = "N314",
                    BranchName = "Vasco de Quiroga",
                    BranchCity = "DF",
                    BranchHour1 = "7:00 AM",
                    BranchHour2 = "11:00 PM",
                    BranchConsult = false,
                    BranchLatitude = "19.381288",
                    BranchLongitude = "-99.240561",
                    BranchActive = true
                },
                new Branch
                {
                    BranchRegion = "R7",
                    BranchCeco = "2L1313",
                    BranchSap = "N313",
                    BranchName = "Insurgentes la Florida",
                    BranchCity = "DF",
                    BranchHour1 = "7:00 AM",
                    BranchHour2 = "11:00 PM",
                    BranchConsult = false,
                    BranchLatitude = "19.366739",
                    BranchLongitude = "-99.181051",
                    BranchActive = true
                },
                new Branch
                {
                    BranchRegion = "R1",
                    BranchCeco = "2L1199",
                    BranchSap = "N199",
                    BranchName = "Satelite II",
                    BranchCity = "Monterrey",
                    BranchHour1 = "7:00 AM",
                    BranchHour2 = "11:00 PM",
                    BranchConsult = false,
                    BranchLatitude = "25.597344",
                    BranchLongitude = "-100.263590",
                    BranchActive = true
                },
                new Branch
                {
                    BranchRegion = "R1",
                    BranchCeco = "2L1197",
                    BranchSap = "N197",
                    BranchName = "Plaza EO5",
                    BranchCity = "Monterrey",
                    BranchHour1 = "",
                    BranchHour2 = "",
                    BranchConsult = false,
                    BranchLatitude = "25.5815",
                    BranchLongitude = "-100.002396",
                    BranchActive = true
                },
                new Branch
                {
                    BranchRegion = "R5",
                    BranchCeco = "2L0893",
                    BranchSap = "M893",
                    BranchName = "Abasolo Allende",
                    BranchAddress =
                        "Benito Juárez No. 902 Norte, entre Abasolo y Libertad, Col. Centro, Allende, Coahuila, C.P. 26530",
                    BranchCity = "Allende, Coah",
                    BranchHour1 = "",
                    BranchHour2 = "",
                    BranchConsult = false,
                    BranchLatitude = "28.3505",
                    BranchLongitude = "-100.853444",
                    BranchActive = true
                },
                new Branch
                {
                    BranchRegion = "R5",
                    BranchCeco = "2L1200",
                    BranchSap = "M200",
                    BranchName = "Jupiter II (Colonial del Valle)",
                    BranchAddress = "Júpiter 810, esq. con Parral, Colonial del Valle, Cd. Juárez, Chihuahua C.P.32553",
                    BranchCity = "Cd Juarez",
                    BranchHour1 = "",
                    BranchHour2 = "",
                    BranchConsult = false,
                    BranchLatitude = "31.6789",
                    BranchLongitude = "-106.362987",
                    BranchActive = true
                },
            };
            branchs.ForEach(a => context.Branchs.AddOrUpdate(h => h.BranchName, a));
            context.SaveChanges();*/
            #endregion

            var firstBranch = context.Branchs.FirstOrDefault();
            if(firstBranch == null)
            {
                context.Database.ExecuteSqlCommand(SqlFileContainer.Branches);
            }
        }

        private static void saveHomePages(MyApplicationDbContext context, List<OfferSection> offerSections, List<CardSection> cardSections,
            List<FoseSection> FoseSections, List<BlogSection> blogSections, Models.Pages.Sections.FourQuadSection fourQuadSection,
            DoctorsOfficeSection doctorsOfficeSection)
        {
            if (!context.HomePages.Any())
            {
                var homePages = new List<HomePage>
                {
                    new HomePage
                    {
                        HomePageCustomValue = "HomePage_1",
                        HomePageTitle = "Inicio",
                        HomePageActive = true,
                        HomePageCreatedDate = DateTime.Now,
                        OfferSectionId = offerSections.Single(g => g.OfferSectionCustomValue == "OfferSection_1").OfferSectionId,
                        CardSectionId = cardSections.Single(g => g.CardSectionCustomValue == "CardSection_1").CardSectionId,
                        FoseSectionId = FoseSections.Single(g => g.FoseSectionCustomValue == "FoseSection_1").FoseSectionId,
                        BlogSectionId = blogSections.Single(g => g.BlogSectionCustomValue == "BlogSection_1").BlogSectionId,
                        FourQuadSection = fourQuadSection,
                        DoctorsOfficeSection = doctorsOfficeSection
                    }
                };
                homePages.ForEach(a => context.HomePages.AddOrUpdate(h => h.HomePageCustomValue, a));
                context.SaveChanges();

                var imageSections = new List<ImageSection>
                {
                    new ImageSection
                    {
                        ImageSectionImage = "~/Content/rsc/imgs/banner.png",
                        ImageSectionPageId = homePages[0].HomePageId,
                        ImageSectionPageName = "HomePage",
                        ImageSectionActive = true
                    }
                };
                imageSections.ForEach(a => context.ImageSections.AddOrUpdate(a));
                context.SaveChanges();
            }
        }

        private static void savePillars(MyApplicationDbContext context)
        {
            if (!context.Pillars.Any())
            {
                var pillars = new List<Pillar>
                {
                    new Pillar
                    {
                        PillarActive = true,
                        PillarDescription = "Actividades enfocadas a mejorar el bienestar de nuestros colaboradores como salud, seguridad, diversidad e inclusion, entre otros",
                        PillarLink = "#",
                        PillarImage = "~/UploadedFiles/empresa_636368765847868310.jpg",
                        PillarName = "empresa",
                    },
                    new Pillar
                    {
                        PillarActive = true,
                        PillarDescription = "Actividades enfocadas a mejorar el bienestar de nuestros colaboradores como salud, seguridad, diversidad e inclusion, entre otros",
                        PillarLink = "#",
                        PillarImage = "~/UploadedFiles/mercado_636368766125914602.jpg",
                        PillarName = "mercado",
                    },
                    new Pillar
                    {
                        PillarActive = true,
                        PillarDescription = "Actividades enfocadas a mejorar el bienestar de nuestros colaboradores como salud, seguridad, diversidad e inclusion, entre otros",
                        PillarLink = "#",
                        PillarImage = "~/UploadedFiles/comunidad_636368765276842317.jpg",
                        PillarName = "comunidad",
                    },
                    new Pillar
                    {
                        PillarActive = true,
                        PillarDescription = "Actividades enfocadas a mejorar el bienestar de nuestros colaboradores como salud, seguridad, diversidad e inclusion, entre otros",
                        PillarLink = "#",
                        PillarImage = "~/UploadedFiles/ambiente_636368753970619568.jpg",
                        PillarName = "ambiente",
                    }
                };
                pillars.ForEach(a => context.Pillars.AddOrUpdate(h => h.PillarName, a));
                context.SaveChanges();
            }
        }

        private static void saveQuotes(MyApplicationDbContext context)
        {
            if (!context.Quotes.Any())
            {
                var quotes = new List<Quote>
                {
                    new Quote
                    {
                        QuoteAuthor = "Erick Saúl Rodríguez",
                        QuoteAuthorSign = "Cajero Sucursal San Jerónimo",
                        QuoteText = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                        QuoteAuthorPhoto = "~/UploadedFiles/fotoErik_636368795263728076.png",
                    },
                };
                quotes.ForEach(a => context.Quotes.AddOrUpdate(h => h.QuoteAuthor, a));
                context.SaveChanges();
            }
        }

        private static void savePillarPages(MyApplicationDbContext context)
        {
            if (!context.PillarPages.Any())
            {
                var pillarPages = new List<PillarPage>
                {
                    new PillarPage
                    {
                        PillarPageCustomValue = "PillarPage_1",
                        PillarPageTitle = "Pilares",
                        PillarPageText1 = "En Farmacias Benavides nos preocupamos por el bienestar de nuestros colaboradores, las comunidades y por el medio ambiente",
                        PillarPageText2 = "Establecimos 4 pilares en los que basamos nuestras actividades, esto para poder enfocar de mejor manera los distintos aspectos sobre los cuales trabajamos:",
                        PillarPageActive = true,
                        PillarPageCreatedDate = DateTime.Now,
                        PillarPageColorText1 = "#ffffff",
                        PillarPageColorText2 = "#ffffff",
                        PillarPageImage = "~/Content/rsc/imgs/bannerPillarPage_636366825455122767.jpg",
                    }
                };
                pillarPages.ForEach(a => context.PillarPages.AddOrUpdate(h => h.PillarPageCustomValue, a));
                context.SaveChanges();
            }
        }

    }
}