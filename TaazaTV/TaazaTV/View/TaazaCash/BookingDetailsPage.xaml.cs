using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaazaTV.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaazaTV.View.TaazaCash
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BookingDetailsPage : ContentPage
    {
        public BookingDetailsPage(AllBookings allBookings)
        {
            InitializeComponent();

            TopImage.Source = allBookings.details.image;
            TopText.Text = allBookings.details.name;
            TopSubText.Text = allBookings.booking_unique_id;
            string[] tokens = allBookings.booking_data_time.Split(' ');
            TransactionDate.Text = tokens[0];
            TicketNumber.Text = allBookings.booking_details.total_number_seat + " ticket(s) booked";
            TranscationAmount.Text = allBookings.booking_details.paid_amount;
            DateBookedeFor.Text = allBookings.booking_details.booking_date_for;
            BookedTimeFor.Text = allBookings.booking_details.booking_time_for;
            Location.Text = allBookings.details.vanue_details.location + allBookings.details.vanue_details.picklocation;

            var html1 = new HtmlWebViewSource
            {
                Html = "<iframe width=\"100%\" height=\"200\" frameborder=\"0\" scrolling=\"no\" marginheight=\"0\" marginwidth=\"0\" src = \"https://maps.google.com/maps?q=" + allBookings.details.vanue_details.latitude + "," + allBookings.details.vanue_details.latitude + "&hl=es;z=14&amp;output=embed\" ></ iframe > "
            };
            BookingLocDesc.Source = html1;

        }
    }
}