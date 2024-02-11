import type { Config } from "drizzle-kit";

export default {
  schema: "./drizzle/schema.ts",
  out: "./drizzle",
  driver: "better-sqlite",
  strict: true,
  verbose: true,
  dbCredentials: {
    url: "./db.sqlite3"
  },
  introspect: {
    casing: "preserve"
  }
} satisfies Config;
