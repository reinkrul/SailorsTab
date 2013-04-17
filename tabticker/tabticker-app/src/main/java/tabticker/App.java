package tabticker;

import org.apache.log4j.Logger;
import tabticker.agents.TabtickerAgent;
import tabticker.services.NotificationService;
import tabticker.services.SoundNotificationService;
import tabticker.services.TwinkleNotificationService;

import java.io.File;
import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

public class App {

    private static final Logger LOG = Logger.getLogger(App.class);

    public static void main(String... args) throws InterruptedException, IOException {
        // AA the text
        System.setProperty("swing.aatext", "true");

        Config config;
        File configFile = new File("config.xml");
        if (configFile.exists()) {
            config = Config.load(configFile);
        } else {
            LOG.info("Config doesn't exist, creating: " + configFile);
            config = Config.create();
            config.save(configFile);
        }

        new App(config);
    }

    public App(Config config) throws InterruptedException, IOException {
        List<NotificationService> services = new ArrayList<NotificationService>();
        services.add(new TwinkleNotificationService(config.getNotificationDuration(), config.getMaxNotifications()));
        if (config.getEnableSounds()) {
            services.add(new SoundNotificationService());
        }

        TabtickerAgent agent = new TabtickerAgent(config.getHttpAgentPort(), services);
        agent.start();
    }
}
