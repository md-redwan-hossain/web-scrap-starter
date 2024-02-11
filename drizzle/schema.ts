import { integer, sqliteTable, text } from "drizzle-orm/sqlite-core";

export const rokomari_data = sqliteTable("rokomari_data", {
  id: integer("id").primaryKey({ autoIncrement: true }),
  book_title: text("book_title"),
  author_name: text("author_name")
});
