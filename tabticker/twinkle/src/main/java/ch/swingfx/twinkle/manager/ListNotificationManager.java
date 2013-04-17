/*
 * This library is dual-licensed: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License version 3 as
 * published by the Free Software Foundation. For the terms of this
 * license, see licenses/gpl_v3.txt or <http://www.gnu.org/licenses/>.
 *
 * You are free to use this library under the terms of the GNU General
 * Public License, but WITHOUT ANY WARRANTY; without even the implied
 * warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details.
 *
 * Alternatively, you can license this library under a commercial
 * license, as set out in licenses/commercial.txt.
 */

package ch.swingfx.twinkle.manager;

import javax.swing.*;
import java.awt.event.WindowAdapter;
import java.awt.event.WindowEvent;
import java.util.LinkedList;

/**
 * Shows a list of notifications
 *
 * @author Rein Krul
 */
public class ListNotificationManager implements INotificationManager {

    private static final int MARGIN = 8;
    private final LinkedList<JWindow> windows = new LinkedList<JWindow>();
    private final int maxNotifications;

    public ListNotificationManager(int maxNotifications) {
        this.maxNotifications = maxNotifications;
    }

    @Override
    public void showNotification(final JWindow window) {
        synchronized (windows) {
            closeExcessNotifications();
            windows.addLast(window);
            window.addWindowListener(new ShowWindowsListener());
            showWindows();
        }
    }

    private void closeExcessNotifications() {
        while (windows.size() >= maxNotifications) {
            JWindow windowToClose = windows.getFirst();
            windowToClose.dispose();
            windows.remove(windowToClose);
        }
    }

    private void showWindows() {
        int offset = 10;
        for (JWindow window : windows) {
            window.setVisible(true);
            window.getGlassPane().setVisible(true);
            window.setLocation(window.getLocation().x, offset);
            offset += window.getPreferredSize().getHeight() + MARGIN;
        }
    }

    private class ShowWindowsListener extends WindowAdapter {
        @Override
        public void windowClosed(WindowEvent e) {
            e.getWindow().removeWindowListener(this);
            synchronized (windows) {
                windows.remove(e.getWindow());
                showWindows();
            }
        }
    }
}
