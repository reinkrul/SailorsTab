package tabticker.services;

import ch.swingfx.twinkle.NotificationBuilder;
import ch.swingfx.twinkle.manager.INotificationManager;
import ch.swingfx.twinkle.manager.ListNotificationManager;
import ch.swingfx.twinkle.style.INotificationStyle;
import ch.swingfx.twinkle.style.theme.DarkDefaultNotification;
import ch.swingfx.twinkle.window.Positions;
import org.apache.log4j.Logger;
import tabticker.Notification;

public class TwinkleNotificationService implements NotificationService {

    private static final Logger LOG = Logger.getLogger(TwinkleNotificationService.class);

    private final INotificationStyle style;

    /**
     * Duration of the notification in milliseconds.
     */
    private final int duration;

    private final INotificationManager notificationManager;

    public TwinkleNotificationService(int duration, int maxNotifications) {
        this.duration = duration;
        this.notificationManager = new ListNotificationManager(maxNotifications);

        // First we define the style/theme of the window.
        // Note how we override the default values
        style = new DarkDefaultNotification()
                .withWidth(400) // Optional
                .withAlpha(0.9f) // Optional
        ;
    }

    public void publish(Notification notification) {
        LOG.debug("Publishing notification: " + notification);

        // Now lets build the notification
        new NotificationBuilder(notificationManager)
                .withStyle(style) // Required. here we set the previously set style
                .withTitle(notification.getTitle()) // Required.
                .withMessage(notification.getText()) // Optional
                .withDisplayTime(duration) // Optional
                .withPosition(Positions.NORTH_EAST)
                .showNotification();
    }
}
