using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

//TODO: Change the namespace here to match your project's name
namespace Team7_LonghornMusic.Models
{
    //enums 
    public enum CardType { None, Visa, MasterCard, Discover, AmericanExpress}
    
    public enum State {
        AK, AL, AR, AZ, CA, CO, CT, DC, DE, FL, GA, HI, IA, ID, IL, IN, KS, KY, LA, MA, MD, ME, MI, MN, MO, MS, MT, NC, ND, NE, NH, NJ, NM, NV, NY, OH, OK, OR, PA, RI, SC, SD, TN, TX, UT, VA, VT, WA, WI, WV, WY
    }

    public class AppUser : IdentityUser
    {
        //TODO: Put any additional fields that you need for your users here
        [Required]
        [Display(Name = "First Name")]
        public string FName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LName { get; set; }
        [Display(Name = "Middle Initial")]
        public string MidInitial { get; set; }

        [Display(Name = "Account enabled?")]
        public bool IsDisabled { get; set; }
        // TODO: Figure out if we need: public int SSN { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public State State { get; set; }
        [Required]
        public string ZipCode { get; set; }

        [Display(Name = "Card One")]
        public string CreditCardOne { get; set; }
        [Display(Name = "Card One Type")]
        public CardType CreditCardTypeOne { get; set; }
        [Display(Name = "Card Two")]
        public string CreditCardTwo { get; set; }
        [Display(Name = "Card Two Type")]
        public CardType CreditCardTypeTwo { get; set; }

        //Navigation Properties
        public virtual List<Song> Songs { get; set;}
        public virtual List<Album> Albums { get; set; }
        public virtual List<ShoppingCart> ShoppingCarts { get; set; }
        public virtual List<ArtistReview> ArtistReviews { get; set; }
        public virtual List<AlbumReview> AlbumReviews { get; set; }
        public virtual List<SongReview> SongReviews { get; set; }
        
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<AppUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }

    //NOTE: Here is your dbContext for the entire project

    public class AppDbContext : IdentityDbContext<AppUser>
    {
        //TODO: Add your dbSets here.  As an example, I've included one for products
        //Remember - the identitydbcontext already contains a db set for users.  
        
        

        public AppDbContext()
            : base("MyDbConnection", throwIfV1Schema: false)
        {
        }

        public static AppDbContext Create()
        {
            return new AppDbContext();
        }

        //Add dbSet for roles
        public DbSet<AppRole> AppRoles { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<SongReview> SongReviews { get; set; }
        public DbSet<ArtistReview> ArtistReviews { get; set; }
        public DbSet<AlbumReview> AlbumReviews { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<Song> Songs { get; set; }

        //public System.Data.Entity.DbSet<Team7_LonghornMusic.Models.AppUser> AppUsers { get; set; }
    }
}