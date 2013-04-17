package tabticker;

public class Notification {

    private final String title;
    private final String text;

    public Notification(String title, String text) {
        this.title = title;
        this.text = text;
    }

    public String getTitle() {
        return title;
    }

    public String getText() {
        return text;
    }

    @Override
    public String toString() {
        return "Notification{" +
                "title='" + title + '\'' +
                ", text='" + text + '\'' +
                '}';
    }
}
