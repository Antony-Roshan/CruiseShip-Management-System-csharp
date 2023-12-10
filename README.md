# CruiseshipApp

Model1 context.cs
public virtual DbSet<Beauty_Saloon> Beauty_Saloon_Table { get; set; }
        public virtual DbSet<Booking_details> Booking_details_Table { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Feedback> Feedbacks { get; set; }
        public virtual DbSet<Fitness_centre> Fitness_centre_Table { get; set; }
        public virtual DbSet<Item> Items_Table { get; set; }
        public virtual DbSet<Login> Logins { get; set; }
        public virtual DbSet<Movie_ticket> Movie_ticket_Table { get; set; }
        public virtual DbSet<Order_details> Order_details { get; set; }
        public virtual DbSet<Order> Orders_Table { get; set; }
        public virtual DbSet<Party_hall> Party_hall_Table { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Voyager> Voyagers { get; set; }
        public virtual DbSet<Movie_bookings> Movie_bookings { get; set; }
        public virtual DbSet<Saloon_booking> Saloon_bookings { get; set; }
