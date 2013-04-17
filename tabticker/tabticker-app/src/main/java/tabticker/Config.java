package tabticker;


import javax.xml.bind.*;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlRootElement;
import java.io.*;

@XmlRootElement
public class Config {

    public static Config create() {
        Config config = new Config();
        config.notificationDuration = 120000;
        config.maxNotifications = 15;
        config.httpAgentPort = 80;
        config.enableSounds = true;
        return config;
    }

    public static Config load(File file) throws IOException {
        try {
            Unmarshaller unmarshaller = JAXBContext.newInstance(Config.class).createUnmarshaller();

            InputStream inputStream = new FileInputStream(file);
            try {
                return (Config) unmarshaller.unmarshal(inputStream);
            } finally {
                inputStream.close();
            }
        } catch (JAXBException e) {
            throw new IOException("Can't load config from " + file, e);
        }
    }

    private int notificationDuration;

    private int maxNotifications;

    private boolean enableSounds;

    private int httpAgentPort;

    public void save(File file) throws IOException {
        try {
            Marshaller marshaller = JAXBContext.newInstance(getClass()).createMarshaller();
            marshaller.setProperty(Marshaller.JAXB_FORMATTED_OUTPUT, Boolean.TRUE);

            OutputStream outputStream = new FileOutputStream(file, false);
            try {
                marshaller.marshal(this, outputStream);
                outputStream.flush();
            } finally {
                outputStream.close();
            }
        } catch (PropertyException e) {
            throw new IOException("Can't save config to " + file, e);
        } catch (JAXBException e) {
            throw new IOException("Can't save config to " + file, e);
        }
    }

    @XmlElement(name = "enable-sounds")
    public boolean getEnableSounds() {
        return enableSounds;
    }

    public void setEnableSounds(boolean enableSounds) {
        this.enableSounds = enableSounds;
    }

    @XmlElement(name = "http-agent-port")
    public int getHttpAgentPort() {
        return httpAgentPort;
    }

    public void setHttpAgentPort(int httpAgentPort) {
        this.httpAgentPort = httpAgentPort;
    }

    @XmlElement(name = "max-notifications")
    public int getMaxNotifications() {
        return maxNotifications;
    }

    public void setMaxNotifications(int maxNotifications) {
        this.maxNotifications = maxNotifications;
    }

    @XmlElement(name = "notification-duration")
    public int getNotificationDuration() {
        return notificationDuration;
    }

    public void setNotificationDuration(int notificationDuration) {
        this.notificationDuration = notificationDuration;
    }
}
