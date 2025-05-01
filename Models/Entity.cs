namespace EntityApi.Models
{
    public class Entity
    {
        public int Id{ get; set;}
        public required string Name {get; set;}
        public required string Surname {get; set;}
        public int Age {get; set;}

        public required string Email {get; set;}

        public string Phone {get; set;} = "";

    }
}