import { UserProfileDto } from "./user-profile-dto";

export interface CorporateDirectoryFilterResultDto {
    userProfileDtos: UserProfileDto[];
    departments: string[];
    count: number;
}