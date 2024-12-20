import {Photo} from"./Photo";
export interface Member{
    id: string;
    userName?: string | null;
    age: number;
    photoUrl?: string | null;
    knownAs: string;
    created: Date;
    lastActive: Date;
    isMale: boolean;
    introduction?: string | null;
    interests?: string | null;
    lookingFor?: string | null;
    city?: string | null;
    country?: string | null;
    photos?: Photo[] | null;
}