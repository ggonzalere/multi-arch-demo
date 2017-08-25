import com.google.gson.Gson;
import java.net.*;
import java.io.*;
import java.util.Base64;
import java.util.concurrent.Executors;
import java.util.concurrent.ScheduledExecutorService;
import java.util.concurrent.TimeUnit;


public class WeatherRaspberry {
    
    public class WeatherForecast {
        public String city;
        public double temperature;
        public String status;
        public String timeStamp;
    }
    
    public class WorldForecast{
        public WeatherForecast[] forecasts;
    }
    
    public static void main(String[] args) {
        final ScheduledExecutorService executorService = Executors.newSingleThreadScheduledExecutor();
           executorService.scheduleAtFixedRate(new Runnable() {
                @Override
                public void run() {
                    getWeather();
                }
            }, 0, 5, TimeUnit.SECONDS);      
    }
    
    public static void getWeather(){
    try{
	System.out.println("Container in Raspberry Pi V2");
        URL url = new URL("yourUrl");
        HttpURLConnection connection = (HttpURLConnection) url.openConnection();
        connection.setRequestMethod("GET");
        connection.setDoOutput(true);
        InputStream content = (InputStream)connection.getInputStream();
            BufferedReader in   = 
                new BufferedReader (new InputStreamReader (content));
            String line;
            while ((line = in.readLine()) != null) {
                Gson gson = new Gson();
                WorldForecast wf = gson.fromJson(line, WorldForecast.class);
                System.out.println(getExtremes(wf));
            }
        } catch(Exception e) {
            System.out.println(e);
            e.printStackTrace();
        }
    }
    
    public static String getExtremes(WorldForecast wf){
        String cha="At "+wf.forecasts[0].timeStamp+":\n";
        for(int i=0; i<wf.forecasts.length; i++){
            if(wf.forecasts[i].status.equals("extremely cold") || wf.forecasts[i].status.equals("extremely hot")){
                cha=cha+wf.forecasts[i].city + " is " + wf.forecasts[i].status + " with " + wf.forecasts[i].temperature + " Celsius\n";
            }
        }
        return cha;
    }
}
