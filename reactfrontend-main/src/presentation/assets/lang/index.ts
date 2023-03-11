import ro from "./ro";
import en from "./en";

export enum SupportedLanguage {
  EN = "en",
  RO = "ro",
}

type ReactIntlMessages = {
  en: Record<string, string>;
  ro: Record<string, string>;
};

const messages: ReactIntlMessages = {
  en,
  ro,
};

export const getMessagesForLanguage = (language: SupportedLanguage) =>
  messages[language];
