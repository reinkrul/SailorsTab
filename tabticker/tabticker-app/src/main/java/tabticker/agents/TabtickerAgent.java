package tabticker.agents;


import com.sun.net.httpserver.HttpExchange;
import com.sun.net.httpserver.HttpHandler;
import com.sun.net.httpserver.HttpServer;
import org.apache.log4j.Logger;
import tabticker.Notification;
import tabticker.services.NotificationService;
import tabticker.util.StringUtils;
import tabticker.util.UrlEncodedQueryString;

import java.io.IOException;
import java.io.OutputStream;
import java.net.InetSocketAddress;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.List;

public class TabtickerAgent {

    private static final Logger LOG = Logger.getLogger(TabtickerAgent.class);

    private final int port;
    private final List<NotificationService> services;

    private Thread shutdownHook;
    private HttpServer server;

    public TabtickerAgent(int port, List<NotificationService> services) {
        this.port = port;
        this.services = services;
    }

    public void start() throws IOException {
        if (server == null) {
            server = HttpServer.create(new InetSocketAddress(port), 10);
            server.createContext("/notification", createNotificationHandler());
            server.setExecutor(null); // allow default executor to be created
            server.start();

            shutdownHook = new Thread(new Runnable() {
                public void run() {
                    stop();
                }
            });
            Runtime.getRuntime().addShutdownHook(shutdownHook);
        }
    }

    public void stop() {
        if (server != null) {
            Runtime.getRuntime().removeShutdownHook(shutdownHook);
            server.stop(0);
            server = null;
            shutdownHook = null;
        }
    }

    private HttpHandler createNotificationHandler() {
        return new HttpHandler() {
            public void handle(HttpExchange httpExchange) throws IOException {
                Notification notification = parseNotification(httpExchange);

                String response;
                int responseCode;
                if (StringUtils.isBlank(notification.getTitle())) {
                    response = "Title can't be empty.";
                    responseCode = 400;
                } else {
                    response = "Notification consumed:\r\n  " + notification;
                    responseCode = 200;

                    LOG.debug("Publishing notification: " + notification);
                    for (NotificationService service : services) {
                        service.publish(notification);
                    }
                }

                httpExchange.sendResponseHeaders(responseCode, response.length());
                OutputStream outputStream = httpExchange.getResponseBody();
                outputStream.write(response.getBytes());
                outputStream.close();
            }
        };
    }

    private static Notification parseNotification(HttpExchange exchange) {
        UrlEncodedQueryString query = UrlEncodedQueryString.parse(exchange.getRequestURI().getQuery());
        return new Notification(new SimpleDateFormat("HH:mm").format(new Date()) + " " + query.get("title"), query.get("text"));
    }
}
