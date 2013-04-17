package tabticker.services;


import org.apache.log4j.Logger;
import tabticker.Notification;

import javax.sound.sampled.*;
import java.io.FileInputStream;
import java.io.IOException;

public class SoundNotificationService implements NotificationService {

    private static final Logger LOG = Logger.getLogger(SoundNotificationService.class);

    private Clip clip = null;
    private boolean playing = false;

    public SoundNotificationService() {
        try {
            clip = AudioSystem.getClip();
            clip.addLineListener(new LineListener() {
                public void update(LineEvent lineEvent) {
                    if (lineEvent.getType() == LineEvent.Type.STOP) {
                        playing = false;
                    }
                }
            });
            clip.open(AudioSystem.getAudioInputStream(new FileInputStream("pop.wav")));
        } catch (LineUnavailableException e) {
            LOG.warn("Can't initialize sound system, service will not be available.", e);
        } catch (IOException e) {
            LOG.warn("Can't initialize sound system, service will not be available.", e);
        } catch (UnsupportedAudioFileException e) {
            LOG.warn("Can't initialize sound system, service will not be available.", e);
        }
    }

    public void publish(Notification notification) {
        if (clip != null) {
            synchronized (clip) {
                if (!playing) {
                    clip.setFramePosition(0);
                    clip.start();
                    playing = true;
                }
            }
        }
    }
}
