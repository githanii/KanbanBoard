export interface Card {
    id: number;
    title: string;
    description: string;
    ListId: number;
    createdAt: Date;
    updatedAt: Date;
    position: number;
    
    }
export interface List {
    id: number;
    title: string;
    BoardId: number;
    order: number;
    createdAt: Date;
    updatedAt: Date;
    cards?: Card[];
    }
export interface BoardDetails {
    id: number;
    name: string;
    createdAt: Date;
    updatedAt: Date;
    lists?: List[];

    }
export interface Board {
  lists: any;
  id: number;
  name: string;
  createdAt?: string;
}
