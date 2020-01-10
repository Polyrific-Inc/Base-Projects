import { UserProfileDto } from "./user-profile-dto";

export interface CorporateDirectoryDto {
    userProfileDtos: UserProfileDto[];
    departments: string[]
}