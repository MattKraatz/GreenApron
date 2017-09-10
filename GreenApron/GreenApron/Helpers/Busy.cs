using Xamarin.Forms;

namespace GreenApron
{
	public static class Busy
    {
		public static ActivityIndicator Flip(ActivityIndicator busy)
		{
			var output = busy;
			output.IsRunning = busy.IsRunning ? false : true;
			output.IsVisible = busy.IsVisible ? false : true;
			return output;
		}
    }
}
