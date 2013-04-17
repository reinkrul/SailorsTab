package tabticker.services;

import tabticker.Notification;

/**
 * Created with IntelliJ IDEA.
 * User: reikrul
 * Date: 4/14/13
 * Time: 1:26 PM
 * To change this template use File | Settings | File Templates.
 */
public interface NotificationService {
    void publish(Notification notification);
}
