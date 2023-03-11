import { SupportedLanguage } from "presentation/assets/lang";
import {
  LanguageContextActions,
  LanguageState,
} from "./LanguageContextProvider.types";

export const languageContextReducer = (
  state: LanguageState,
  action: LanguageContextActions
): LanguageState => {
  switch (action.type) {
    case "setRomanianLanguage":
      return {
        ...state,
        selectedLanguage: SupportedLanguage.RO,
      };
    case "setEnglishLanguage":
      return {
        ...state,
        selectedLanguage: SupportedLanguage.EN,
      };
    default:
      return state;
  }
};
